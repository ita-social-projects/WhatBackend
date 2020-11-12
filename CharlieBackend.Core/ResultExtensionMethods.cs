using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Core
{
    public static class ResultExtensionMethods
    {
        /// <summary>
        /// Used to return data or error ActionResult (status code) format in controller
        /// </summary>
        /// <typeparam name="T">Type of transferred data</typeparam>
        /// <param name="result">Result T variable with data or error to transfer to consumer from controller</param>
        /// <returns>Status code depending Result T data. If ErrorData is not empty, return error status code.
        /// If no error transferred, returns OkResult (200 or 204 status code)</returns>
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (Equals(result, null))
            {
                return new ObjectResult("No data received while processing data model") { StatusCode = 500 };
            }
            else if (result.ErrorData != null)
            {
                switch (result.ErrorData.ErrorCode)
                {
                    case ErrorCode.Unauthorized:
                        return new UnauthorizedObjectResult(result.ErrorData.ErrorMessage);//401
                    case ErrorCode.ValidationError:
                        return new BadRequestObjectResult(result.ErrorData.ErrorMessage);//400
                    case ErrorCode.InternalServerError:
                        return new StatusCodeResult(500);//500
                    case ErrorCode.NullReference:
                        return new BadRequestResult();//400
                    case ErrorCode.NotFound:
                        return new NotFoundObjectResult(result.ErrorData.ErrorMessage);//404
                    case ErrorCode.UnprocessableEntity:
                        return new UnprocessableEntityObjectResult(result.ErrorData.ErrorMessage);//422
                    case ErrorCode.Conflict:
                        return new ConflictObjectResult(result.ErrorData.ErrorMessage);//409
                    default:
                        return new StatusCodeResult(500);
                }
            }
            else if (!object.Equals(result.Data, default(T)))
            {
                return new OkObjectResult(result.Data);
            }
            else
            {
                return new OkResult();
            }

        }
    }
}
