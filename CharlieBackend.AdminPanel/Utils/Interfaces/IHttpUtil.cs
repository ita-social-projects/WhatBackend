using CharlieBackend.Core.DTO.Account;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils.Interfaces
{
    public interface IHttpUtil
    {
        Task<HttpResponseMessage> GetAsync(string url, string accessToken = null);

        Task<HttpResponseMessage> PostJsonAsync<T>(string url, T postData, string accessToken = null);

        Task<HttpResponseMessage> PutJsonAsync<T>(string url, T postData, string accessToken = null);

        Task<HttpResponseMessage> DeleteAsync(string url, string accessToken = null);

        void EnsureSuccessStatusCode(HttpResponseMessage httpResponse);

    }
}
