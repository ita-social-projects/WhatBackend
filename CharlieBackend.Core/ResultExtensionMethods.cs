﻿using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core.DTO.Result;
using AutoMapper;

namespace CharlieBackend.Core
{
    public static class ResultExtensionMethods
    {
        /// <summary>
        /// Used to return data or error ActionResult (status code) format in controller
        /// </summary>
        /// <typeparam name="T">Type of transferred data</typeparam>
        /// <param name="result">Result T variable with data or error to transfer to consumer from controller</param>
        /// <returns>Status code depending Result T data. If ErrorData is not empty, return error status code,
        /// and error message description in Json format.
        /// If no error transferred, returns OkResult (200 or 204 status code)</returns>
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (Equals(result, null))
            {
                result = new Result<T>
                {
                    Error = new ErrorData()
                    {
                        Code = ErrorCode.InternalServerError,
                        Message = "No data received while processing data model"
                    }
                };

                return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 500 };
            }
            else if (result.Error != null)
            {
                switch (result.Error.Code)
                {
                    case ErrorCode.Unauthorized:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 401 };//401
                    case ErrorCode.ValidationError:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 400 };//400
                    case ErrorCode.InternalServerError:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 500 };//500
                    case ErrorCode.NotFound:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 404 };//404
                    case ErrorCode.UnprocessableEntity:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 422 };//422
                    case ErrorCode.Conflict:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 409 };//409
                    default:
                        return new JsonResult(ToErrorDto(result.Error)) { StatusCode = 500 };
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

            ErrorDto ToErrorDto(ErrorData errorData)
            {
                var errorDtoData = new ErrorDto
                {
                    Error = errorData
                };

                return errorDtoData;
            }
        }
    }
}
