using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Models.ResultModel
{
    public class Result<T>
    {

        public T TransferredData { get; set; }

        public Error Error { get; set; }

        public static Result<T> ReturnData(T transferredData)
        {
            if (transferredData == null)
            {
                return new Result<T>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.BadRequest,
                        ErrorMessage = "Data is not given"
                    }
                };
            }
            else
            {
                return new Result<T>
                {
                    TransferredData = transferredData,
                    Error = null
                };
            }
        }

        public static Result<T> ReturnError(ErrorCode errorCode, string errorMessage)
        {
            if (errorCode == 0)
            {
                var newResult = new Result<T>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.InternalError,
                        ErrorMessage = "Wrong error model"
                    },
                    TransferredData = default(T)
                };

                return newResult;
            }
            else
            {
                var newResult = new Result<T>
                {
                    Error = new Error
                    {
                        ErrorCode = errorCode,
                        ErrorMessage = errorMessage
                    },
                    TransferredData = default(T)
                };

                return newResult;
            }
        }
    }
}
