using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Newtonsoft.Json;
using CharlieBackend.AdminPanel.Exceptions;
using CharlieBackend.Core.DTO.Result;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;

namespace CharlieBackend.AdminPanel.Utils
{
    public class HttpUtil : IHttpUtil
    {
        private readonly HttpClient _client;

        public HttpUtil(IOptions<ApplicationSettings> config,
                        IHttpContextAccessor httpContextAccessor,
                        IDataProtectionProvider provider)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(config.Value.Urls.Api.Https)
            };

            string protectedToken = httpContextAccessor.HttpContext.Request.Cookies["accessToken"];

            if(!string.IsNullOrEmpty(protectedToken))
            {
                IDataProtector protector = provider.CreateProtector(config.Value.Cookies.SecureKey);

                string token = protector.Unprotect(protectedToken);

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage httpResponse = await _client.SendAsync(requestMessage);

            return httpResponse;
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T postData)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

        public async Task<HttpResponseMessage> PutJsonAsync<T>(string url, T postData)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);

            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

        public async Task EnsureSuccessStatusCode(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                string apiStringResponse = await httpResponse.Content.ReadAsStringAsync();

                var apiResponseMessage = JsonConvert.DeserializeObject<ErrorDto>(apiStringResponse).Error.Message;

                throw new HttpStatusException(httpResponse.StatusCode, apiResponseMessage);
            }
        }

        public async Task<HttpResponseMessage> PatchAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, url);

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

    }
}
