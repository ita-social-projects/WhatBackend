using System;

namespace CharlieBackend.Core.Models.ResultModel
{

    /// <summary>
    /// Used to transfer data or errors information thrue app layers. Methods has to return Result T data type
    /// </summary>
    /// <typeparam name="T">Generic type of data type transferred</typeparam>
    public class Result<T>
    {
        public T Data { get; set; }

        public ErrorData ErrorData { get; set; }

        /// <summary>
        /// If followed code assume returning data without errors, use Success method. 
        /// It will pack your data into Return data type.
        /// </summary>
        /// <param name="transferredData">Any data type to return in calling method, 
        /// which will be converted into Return type</param>
        /// <returns></returns>
        ///<exception cref="ArgumentNullException">Exception thrown if transferred data is empty or null</exception>
        public static Result<T> GetSuccess(T transferredData)
        {
            
            if (object.Equals(transferredData, default(T) ) && typeof(T) != typeof(bool)) // default(bool) is false
            {
                throw new ArgumentNullException();
            }
            else
            {
                var transferredDataToReturn = new Result<T>()
                {
                    Data = transferredData,
                };

                return transferredDataToReturn;
            }
        }

        /// <summary>
        /// If followed code assume returning error, use Error method to return Error. 
        /// It will convert data into Return T data type.
        /// </summary>
        public static Result<T> GetError(ErrorCode errorCode, string errorMessage)
        {
            var newResult = new Result<T> 
            { 
                ErrorData = new ErrorData
                {
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage
                },
            };

            return newResult;
        }
    }
}
