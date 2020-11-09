using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils
{
    public class ApiUtil: IApiUtil
    {
        private IHttpUtil _httpUtil;

        public ApiUtil(IHttpUtil httpUtil)
        {
            _httpUtil = httpUtil;
        }

        public async Task<T> SignInAsync<T>(string url, AuthenticationDto authModel)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, authModel);


            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            // return httpResponse.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(); // get auth token;

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }

        public async Task<T> GetAsync<T>(string url, string accessToken)
        {
            var httpResponse = await _httpUtil.GetAsync(url, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }

        public async Task<T> CreateAsync<T>(string url, T data, string accessToken)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); 

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }

        public async Task<T> PutAsync<T>(string url, T data, string accessToken)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); 

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }


        public async Task<T> DeleteAsync<T>(string url, string accessToken)
        {
            var httpResponse = await _httpUtil.DeleteAsync(url, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); 

            var responseModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responseModel;
        }

      
    }
}
