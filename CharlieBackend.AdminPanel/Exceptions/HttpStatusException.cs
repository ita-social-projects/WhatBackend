using System;
using System.Net;

namespace CharlieBackend.AdminPanel.Exceptions
{
    class HttpStatusException : Exception
    {

        public HttpStatusCode HttpStatusCode { get; set; }

        public HttpStatusException(HttpStatusCode statusCode, string message) : base(message)
        {
            HttpStatusCode = statusCode;
        }
    }
}
