using CharlieBackend.Core.Models.Account;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils.Interfaces
{
    public interface IApiUtil
    {
        public Task<T> SignInAsync<T>(string url, AuthenticationModel authModel);

        public Task<T> GetAsync<T>(string url, string accessToken);

        public Task<T> CreateAsync<T>(string url, T data, string accessToken);

        public Task<T> PutAsync<T>(string url, T data, string accessToken);

        public Task<T> DeleteAsync<T>(string url, string accessToken);
    }
}
