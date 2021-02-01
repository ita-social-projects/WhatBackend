using CharlieBackend.Core.DTO.Account;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils.Interfaces
{
    public interface IApiUtil
    {
        public Task<string> SignInAsync(string url, AuthenticationDto authModel);

        public Task<T> GetAsync<T>(string url);

        public Task<T> CreateAsync<T>(string url, T data);

        public Task<T> PutAsync<T>(string url, T data);

        public Task<T> DeleteAsync<T>(string url);
    }
}
