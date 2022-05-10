using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Panel.Models.Account;
using CharlieBackend.Panel.Utils.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Utils
{
    public class ApiUtil : IApiUtil, IBinaryResultApiUtil
    {
        private IHttpUtil _httpUtil;

        public ApiUtil(IHttpUtil httpUtil)
        {
            _httpUtil = httpUtil;
        }

        public async Task<SignInResultDto> SignInAsync(string url, AuthenticationDto authModel)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, authModel);

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

        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var httpResponse = await _httpUtil.GetAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);


            return responseModel;
        }

        public async Task<TResponse> CreateAsync<TResponse>(string url, TResponse data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> CreateAsync<TResponse, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data);

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

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> PutAsync<TResponse, TRequest>(string url, TRequest data)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }


        public async Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            var httpResponse = await _httpUtil.DeleteAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);

            return responseModel;
        }

        public async Task<TResponse> EnableAsync<TResponse>(string url)
        {
            var httpResponse = await _httpUtil.PatchAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync();

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(stringResponse);
            
            return responseModel;
        }

        public async Task<byte[]> PostAsync<TPostData>(string url, TPostData postData)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, postData);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            var binaryResult = await httpResponse.Content.ReadAsByteArrayAsync();

            return binaryResult;
        }

        public async Task<byte[]> GetAsync(string url)
        {
            var httpResponse = await _httpUtil.GetAsync(url);

            await _httpUtil.EnsureSuccessStatusCode(httpResponse);

            var binaryResult = await httpResponse.Content.ReadAsByteArrayAsync();

            return binaryResult;
        }
    }
}
