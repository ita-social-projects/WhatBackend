﻿using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Exceptions;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;

namespace CharlieBackend.AdminPanel.Utils
{
    public class HttpUtil : IHttpUtil
    {
        private readonly HttpClient _client;

        public HttpUtil()
        {
            _client = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url, string accessToken = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            if (!String.IsNullOrEmpty(accessToken))
            {
                requestMessage.Headers.Add("Authorization", accessToken);
            }

            HttpResponseMessage httpResponse = await _client.SendAsync(requestMessage);

            return httpResponse;
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T postData, string accessToken = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            if (!String.IsNullOrEmpty(accessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
            }


            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

        public async Task<HttpResponseMessage> PutJsonAsync<T>(string url, T postData, string accessToken = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);

            if (!String.IsNullOrEmpty(accessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
            }

            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, string accessToken = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            if (!String.IsNullOrEmpty(accessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
            }

            var responseMessage = await _client.SendAsync(requestMessage);

            return responseMessage;
        }

    }
}
