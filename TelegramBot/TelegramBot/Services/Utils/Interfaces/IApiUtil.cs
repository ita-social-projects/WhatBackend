using System.Threading.Tasks;
using TelegramBot.Models.Accounts;

namespace TelegramBot
{
    public interface IApiUtil
    {
        public Task<SignInResultDto> SignInAsync(string url, string telegramId);

        public Task<TResponse> GetAsync<TResponse>(string url);

        public Task<TResponse> CreateAsync<TResponse>(string url, TResponse data);

        public Task<TResponse> CreateAsync<TResponse, TRequest>(string url, TRequest data);

        public Task<TResponse> PostAsync<TResponse, TRequest>(string url, TRequest data);

        public Task<TResponse> PutAsync<TResponse>(string url, TResponse data);

        public Task<TResponse> PutAsync<TResponse, TRequest>(string url, TRequest data);

        public Task<TResponse> DeleteAsync<TResponse>(string url);

        public Task<TResponse> EnableAsync<TResponse>(string url);
    }
}
