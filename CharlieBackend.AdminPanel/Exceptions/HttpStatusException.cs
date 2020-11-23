using CharlieBackend.Core.DTO.Result;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace CharlieBackend.AdminPanel.Exceptions
{
    class HttpStatusException : Exception
    {
        private static string GetFormattedMessage(HttpResponseMessage httpResponseMessage)
        {
            string stringResponse = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var message = $"{JsonConvert.DeserializeObject<ErrorDto>(stringResponse).Error.Message}";

            return message;
        }

        public HttpStatusCode HttpStatusCode;

        public HttpStatusException(HttpResponseMessage httpResponseMessage) : base(GetFormattedMessage(httpResponseMessage))
        {
            HttpStatusCode = httpResponseMessage.StatusCode;
        }
    }
}
