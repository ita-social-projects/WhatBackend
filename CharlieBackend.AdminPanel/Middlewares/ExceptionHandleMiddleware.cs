using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CharlieBackend.AdminPanel.Middlewares

{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger = null;

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
            catch (HttpRequestException ex)
            {
                Regex regex = new Regex(@"\d{3}");

                _logger.LogError($"Something went wrong with API: {ex}");

                context.Response.Redirect($"/Home/ApiError/{regex.Match(ex.Message).Value}");
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/Home/ApiError/500");

                _logger.LogError($"Internal Server Error: {ex.Message}");
            }
        }

    }
}
