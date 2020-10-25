using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Models.ResultModel
{
    public class Result<T>
    {

        public Task<T> TransferredData { get; private set; }

        public Error Error { get; private set; }

        public static Task<Result<T>> ReturnData(Task<T> transferredData)
        {
            if (transferredData == null)
            {
                var nullDataToTransfer = new Result<T>() 
                { 
                    TransferredData = default, 
                    Error = new Error() 
                    { 
                        ErrorCode = ErrorCode.BadRequest, 
                        ErrorMessage = "Data is not given" 
                    } 
                };

                return Task.FromResult(nullDataToTransfer);
            }
            else
            {
                var transferredDataToReturn = new Result<T>()
                {
                    Error = default,
                    TransferredData = transferredData,
                };

                return Task.FromResult(transferredDataToReturn);
            }
        }

        public static Task<Result<T>> ReturnError(ErrorCode errorCode, string errorMessage)
        {
            if (errorCode == ErrorCode.None)
            {
                var newResult = new Result<T>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.InternalError,
                        ErrorMessage = "Wrong error information model"
                    },
                    TransferredData = default
                };

                return Task.FromResult(newResult);
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
                    TransferredData = default
                };

                return Task.FromResult(newResult);
            }
        }
    }
}
