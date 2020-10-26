using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Core.Models.ResultModel
{
    /// <summary>
    /// Class is used to transfer data or errors information. Methods has to return Result<T> data type
    /// </summary>
    /// <typeparam name="T">type of data transferred thrue Result class</typeparam>
    public class Result<T>
    {
        public T TransferredData { get; set; }

        public ErrorData ErrorData { get; set; }

        /// <summary>
        /// In case if followed code assume returning data without errors, use Success method. It will convert data into Return<T> data type.
        /// </summary>
        /// <param name="transferredData">any data type to return in calling method, which will be converted into Return type</param>
        /// <returns></returns>
        public static Result<T> Success(T transferredData)
        {
            ///<exception cref="System.ArgumentNullException">Exception thrown if transferred data is empty/null</exception>
            if (transferredData == default)
            {
                throw new ArgumentNullException();
            }
            else
            {
                var transferredDataToReturn = new Result<T>()
                {
                    ErrorData = default,
                    TransferredData = transferredData,
                };

                return transferredDataToReturn;
            }
        }

        /// <summary>
        /// In case if followed code assume returning error, use Error method to return Error. It will convert data into Return<T> data type.
        /// </summary>
        public static Result<T> Error(ErrorCode errorCode, string errorMessage)
        {
                var newResult = new Result<T>
                {
                    ErrorData = new ErrorData
                    {
                        ErrorCode = errorCode,
                        ErrorMessage = errorMessage
                    },
                    TransferredData = default
                };

                return newResult;
        }
    }
}
