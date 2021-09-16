using System;
using System.IO;
using System.Threading.Tasks;

namespace CharlieBackend.Business.Services.FileServices.ExportFileServices
{
    /// <summary>
    /// Class for general file methods and fields
    /// Remember to implement and use IDisposable.Dispose() for inherited classes
    /// </summary>
    public abstract class BaseFileExport : IDisposable
    {//TODO
        protected MemoryStream _memoryStream;

        /// <summary>
        /// Method for filename
        /// </summary>
        /// <returns>(string) filename </returns>
        public virtual string GetFileName()
        {
            return "Filename";
        }

        /// <summary>
        /// Method for content type
        /// </summary>
        /// <returns>(string) content type</returns>
        public virtual string GetContentType()
        {
            return "application/octet-stream";
        }

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public virtual void Dispose()
        {
            _memoryStream.Dispose();
        }

        /// <summary>
        /// Method that converts data into byte array for file
        /// </summary>
        /// <returns>(byte[]) byte array</returns>
        public virtual async Task<byte[]> GetByteArrayAsync()
        {
            if(_memoryStream == null)
            {
                return await Task.Run(() => { return new byte[0]; });
            }

            return await Task.Run(() => { return _memoryStream.ToArray(); });
        }
    }
}
