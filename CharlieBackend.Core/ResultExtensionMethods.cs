using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core
{
    public static class ResultExtensionMethods
    {
        public static IActionResult ResultToActionResult<T>(this Result<T> result)
        {
            if (Equals(result, default(T)))
            {
                return new ObjectResult("No error data received while processing error") { StatusCode = 500 };
            }
            else
            {
                switch (result.Error.ErrorCode)
                {
                    case ErrorCode.NullReference: //500
                        return new ObjectResult(result.Error.ErrorMessage) { StatusCode = 500 };
                    case ErrorCode.InternalError: //500
                        return new ObjectResult(result.Error.ErrorMessage) { StatusCode = 500 };
                    case ErrorCode.DataNotFound: //400
                        return new ObjectResult(result.Error.ErrorMessage) { StatusCode = 400 };
                    case ErrorCode.Unauthorised: //401
                        return new UnauthorizedObjectResult(result.Error.ErrorMessage);
                    case ErrorCode.NotEnoughtRights: //401
                        return new UnauthorizedObjectResult(result.Error.ErrorMessage);
                    case ErrorCode.BadRequest: //400
                        return new BadRequestObjectResult(result.Error.ErrorMessage);
                    case ErrorCode.Conflict: //409
                        return new ConflictObjectResult(result.Error.ErrorMessage);
                    case ErrorCode.Forbidden: //409
                        return new ForbidResult(result.Error.ErrorMessage);
                    case ErrorCode.UnprocessableEntity: //422
                        return new UnprocessableEntityObjectResult(result.Error.ErrorMessage);
                    case ErrorCode.ValidationProblem: //400
                        return new BadRequestObjectResult(result.Error.ErrorMessage);
                    default:
                        return new StatusCodeResult(500);
                }
            }

        }
    }
}
