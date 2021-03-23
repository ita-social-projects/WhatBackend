using System;
using System.Text.Json;
using System.Threading.Tasks;
using CharlieBackend.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CharlieBackend.Api.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionHandleMiddleware
    {
        #region
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger = null;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
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

            switch(exception.GetType().Name)
            {
                case nameof(EntityValidationException):
                    context.Response.StatusCode = 400;
                    break;
                case nameof(NotFoundException):
                    context.Response.StatusCode = 404;
                    break;
                default:
                context.Response.StatusCode = 500;
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize (new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. " + exception.Message
            }));
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
