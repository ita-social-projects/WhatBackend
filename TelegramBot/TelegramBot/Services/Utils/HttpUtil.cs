using CharlieBackend.Core.DTO.Result;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TelegramBot;

namespace TelegramBot.Utils
{
    public class HttpUtil : IHttpUtil
    {
        private readonly HttpClient _client;

        public HttpUtil(IOptions<ApplicationSettings> config,
                        IHttpContextAccessor httpContextAccessor,
                        IDataProtectionProvider provider,
                        HttpClient client)
        {

            _client = client;

            //string protectedToken = httpContextAccessor.HttpContext != null ? httpContextAccessor.HttpContext.Request.Cookies["accessToken"] : null;
             string protectedToken = httpContextAccessor.HttpContext.Request.Cookies["accessToken"];

            if (!string.IsNullOrEmpty(protectedToken))
            {
                IDataProtector protector = provider.CreateProtector(config.Value.Cookies.SecureKey);

                string token = protector.Unprotect(protectedToken);

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

        public async Task<string> EnsureSuccessStatusCode(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                string apiStringResponse = await httpResponse.Content.ReadAsStringAsync();

                //ToDo: ErrorsConstants.EmptyApiStringResponse or use resources file
                return !string.IsNullOrWhiteSpace(apiStringResponse)
                    ? JsonConvert.DeserializeObject<ErrorDto>(apiStringResponse).Error.Message
                    : "EmptyApiStringResponse";
            }

            return string.Empty;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage httpResponse = await _client.SendAsync(requestMessage);

            return httpResponse;
        }

        public async Task<HttpResponseMessage> PatchAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, url);

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
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
    }
}
