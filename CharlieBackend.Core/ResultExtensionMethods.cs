using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models.ResultModel;

namespace CharlieBackend.Core
{
    public static class ResultExtensionMethods
    {
        public static ActionResult ResultToActionResult<T>(this Result<T> result)
        {
            if (Equals(result, null))
            {
                return new ObjectResult("No data received while processing data model") { StatusCode = 500 };
            }
            else if (result.ErrorData != null)
            {
                switch (result.ErrorData.ErrorCode)
                {
                    case ErrorCode.Unauthorized: //401
                        return new UnauthorizedObjectResult(result.ErrorData.ErrorMessage);
                    case ErrorCode.ValidationError: //400
                        return new BadRequestObjectResult(result.ErrorData.ErrorMessage);
                    case ErrorCode.None: //500
                        return new StatusCodeResult(500);
                    default:
                        return new StatusCodeResult(500);
                }
            }
            else if (!object.Equals(result.TransferredData, default(T)))
            {
                return new OkObjectResult(result.TransferredData);
            }
            else
            {
                return new OkResult();
            }

        }
    }
}
