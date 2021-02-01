using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Newtonsoft.Json;
using CharlieBackend.AdminPanel.Exceptions;
using CharlieBackend.Core.DTO.Result;
using System.Net.Http.Headers;

namespace CharlieBackend.AdminPanel.Utils
{
    public class HttpUtil : IHttpUtil
    {
        private readonly HttpClient _client;

        public HttpUtil(string baseUrl, string token)
        {
            _client = new HttpClient() 
            { 
                BaseAddress = new Uri(baseUrl) 
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public HttpUtil(string baseUrl)
        {
            _client = new HttpClient() 
            { 
                BaseAddress = new Uri(baseUrl) 
            };
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

    }
}
