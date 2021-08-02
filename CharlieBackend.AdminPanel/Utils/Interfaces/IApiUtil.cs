using CharlieBackend.Core.DTO.Account;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils.Interfaces
{
    public interface IApiUtil
    {
        public Task<string> SignInAsync(string url, AuthenticationDto authModel);

        public Task<TResponce> GetAsync<TResponce>(string url);

        public Task<TResponce> CreateAsync<TResponce>(string url, TResponce data);

        public Task<TResponce> CreateAsync<TResponce, TRequest>(string url, TRequest data);

        public Task<TResponce> PostAsync<TResponce, TRequest>(string url, TRequest data);

        public Task<TResponce> PutAsync<TResponce>(string url, TResponce data);

        public Task<TResponce> PutAsync<TResponce, TRequest>(string url, TRequest data);

        public Task<TResponce> DeleteAsync<TResponce>(string url);

        public Task<TResponce> EnableAsync<TResponce>(string url);
    }
}
