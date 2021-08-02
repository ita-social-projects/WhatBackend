using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils
{
    public class ApiUtil : IApiUtil
    {
        private IHttpUtil _httpUtil;

        public ApiUtil(IHttpUtil httpUtil)
        {
            _httpUtil = httpUtil;
        }

        public async Task<string> SignInAsync(string url, AuthenticationDto authModel)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, authModel);

            if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }

            httpResponse.Headers.TryGetValues("Authorization", out IEnumerable<string> token);

            if (token == null)
            {
                return null;
            }

            return token.FirstOrDefault();
        }

        public async Task<TResponce> GetAsync<TResponce>(string url)
        {
            var httpResponse = await _httpUtil.GetAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);


            return responseModel;
        }

        public async Task<TResponce> CreateAsync<TResponce>(string url, TResponce data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);

            return responseModel;
        }

        public async Task<TResponce> CreateAsync<TResponce, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);

            return responseModel;
        }

        public async Task<TResponce> PostAsync<TResponce, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);

            return responseModel;
        }

        public async Task<TResponce> PutAsync<TResponce>(string url, TResponce data)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);

            return responseModel;
        }

        public async Task<TResponce> PutAsync<TResponce, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);

            return responseModel;
        }


        public async Task<TResponce> DeleteAsync<TResponce>(string url)
        {
            var httpResponse = await _httpUtil.DeleteAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);

            return responseModel;
        }

        public async Task<TResponce> EnableAsync<TResponce>(string url)
        {
            var httpResponse = await _httpUtil.PatchAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponce responseModel = JsonConvert.DeserializeObject<TResponce>(stringResponse);
            
            return responseModel;
        }
    }
}
