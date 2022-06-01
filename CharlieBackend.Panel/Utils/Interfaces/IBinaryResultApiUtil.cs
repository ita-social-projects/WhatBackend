using System.Threading.Tasks;

namespace CharlieBackend.Panel.Utils.Interfaces
{
    /// <summary>
    /// Provides mechanism to send HTTP requests and obtain binary result
    /// </summary>
    public interface IBinaryResultApiUtil
    {
        /// <summary>
        /// Called to send asynchronous POST request and obtain binary result
        /// </summary>
        /// <typeparam name="TPostData"></typeparam>
        /// <param name="url">Api endpoint url</param>
        /// <param name="postData">Object that will be serialized to JSON and put to request body</param>
        /// <returns>Byte array</returns>
        Task<byte[]> PostAsync<TPostData>(string url, TPostData postData);

        /// <summary>
        /// Called to send asynchronous GET request and obtain binary result
        /// </summary>
        /// <param name="url">Api endpoint url</param>
        /// <returns>Byte array</returns>
        Task<byte[]> GetAsync(string url);
    }
}
