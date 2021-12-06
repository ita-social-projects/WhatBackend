using CharlieBackend.Panel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web;

namespace CharlieBackend.Panel.Middlewares

{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

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
            catch (HttpStatusException ex)
            {
                _logger.LogError($"Something went wrong with API. {ex.Message}");
                    
                var encodedMessage = HttpUtility.UrlEncode(ex.Message);

                context.Response.Redirect($"/Home/ApiError/{(uint)ex.HttpStatusCode}/{encodedMessage}");
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/Home/ApiError/500/Internal Server Error");

                _logger.LogError($"Internal Server Error: {ex.Message}");
                _logger.LogError(ex.StackTrace);
            }
        }

    }
}
