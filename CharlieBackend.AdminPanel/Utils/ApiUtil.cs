using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using System.Linq;
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

            httpResponse.EnsureSuccessStatusCode(); 

            return httpResponse.Headers?.FirstOrDefault(x => x.Key == "Authorization").Value?.FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string url, string accessToken)
        {
            var httpResponse = await _httpUtil.GetAsync(url, accessToken);

            httpResponse.EnsureSuccessStatusCode();

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }

        public async Task<T> CreateAsync<T>(string url, T data, string accessToken)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data, accessToken);

            httpResponse.EnsureSuccessStatusCode();

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }

        public async Task<T> PutAsync<T>(string url, T data, string accessToken)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data, accessToken);

            httpResponse.EnsureSuccessStatusCode();

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }


        public async Task<T> DeleteAsync<T>(string url, string accessToken)
        {
            var httpResponse = await _httpUtil.DeleteAsync(url, accessToken);

            httpResponse.EnsureSuccessStatusCode();

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }


    }
}
