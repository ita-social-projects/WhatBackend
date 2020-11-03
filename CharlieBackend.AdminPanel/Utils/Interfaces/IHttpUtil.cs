using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Utils.Interfaces
{
    public interface IHttpUtil
    {
        public Task<HttpResponseMessage> GetAsync(string url, string accessToken = null);

        public Task<HttpResponseMessage> PostJsonAsync<T>(string url, T postData, string accessToken = null);

        public Task<HttpResponseMessage> PutJsonAsync<T>(string url, T postData, string accessToken = null);

        public Task<HttpResponseMessage> DeleteAsync(string url, string accessToken = null);
    }
}
