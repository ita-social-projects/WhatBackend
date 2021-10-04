using CharlieBackend.Core.DTO.Account;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Utils.Interfaces
{
    public interface IHttpUtil
    {
        Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> PostJsonAsync<T>(string url, T postData);

        Task<HttpResponseMessage> PutJsonAsync<T>(string url, T postData);

        Task<HttpResponseMessage> DeleteAsync(string url);

        Task EnsureSuccessStatusCode(HttpResponseMessage httpResponse);

        public Task<HttpResponseMessage> PatchAsync(string url);
    }
}
