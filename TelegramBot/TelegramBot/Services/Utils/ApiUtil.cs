using CharlieBackend.Core.DTO.Account;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TelegramBot.Models.Accounts;
using TelegramBot;

namespace TelegramBot.Utils
{
    public class ApiUtil : IApiUtil
    {
        private string _accessToken;
        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                _httpUtil.SetAuthorizationToken(value);
            }
        }

        private IHttpUtil _httpUtil;

        public ApiUtil(IHttpUtil httpUtil)
        {
            _httpUtil = httpUtil;
            _httpUtil.SetAuthorizationToken(AccessToken);
        }

        public async Task<TResponse> CreateAsync<TResponse>(string url, TResponse data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await EnsureSuccessStatusCodeAsync(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> CreateAsync<TResponse, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await EnsureSuccessStatusCodeAsync(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            var httpResponse = await _httpUtil.DeleteAsync(url);

            await EnsureSuccessStatusCodeAsync(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> EnableAsync<TResponse>(string url)
        {
            var httpResponse = await _httpUtil.PatchAsync(url);

            await EnsureSuccessStatusCodeAsync(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var httpResponse = await _httpUtil.GetAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> PutAsync<TResponse>(string url, TResponse data)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data);

            await EnsureSuccessStatusCodeAsync(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> PutAsync<TResponse, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data);

            await EnsureSuccessStatusCodeAsync(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<SignInResultDto> SignInAsync(string url, string telegramId)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, telegramId);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<AuthenticationResponseDto>(stringResponse);

            httpResponse.Headers.TryGetValues("Authorization", out IEnumerable<string> token);

            var result = new SignInResultDto
            {
                RoleList = responseModel.RoleList,
                Token = token.FirstOrDefault()
            };

            return result;
        }

        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage httpResponse)
        {
            string message = await _httpUtil.EnsureSuccessStatusCode(httpResponse);
            if (!string.IsNullOrEmpty(message))
            {
                throw new HttpStatusException(httpResponse.StatusCode, message);
            }
        }
    }
}
