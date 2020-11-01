using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        #region
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger = null;
        #endregion

        public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            
            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. " + exception.Message
            }.ToString());
        }

        private class ErrorDetails
        {
            public ErrorDetails()
            {
            }

            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    }
}