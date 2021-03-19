using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Middlewares
{
    /// <summary>
    /// Middleware that restricts requests from users with deactivated accounts.
    /// </summary>
    public class IsAccountActiveMiddleware
    {
        private readonly RequestDelegate _next;

        public IsAccountActiveMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(
            HttpContext context, 
            IAccountService accountService, 
            ICurrentUserService currentUserService)
        {
            if (context.Request.Path.Value.Contains("accounts")
                || context.Request.Path.Value.Contains("swagger"))
            {
                await _next.Invoke(context);
            }
            else
            {
                var currentEmail = currentUserService.Email;

                var isActive = await accountService.IsAccountActiveAsync(currentEmail);
                    
                if ((bool)!isActive)
                {
                     context.Response.StatusCode = StatusCodes.Status403Forbidden;

                     await context.Response.WriteAsync("Account is deactivated");
                }
                
                await _next.Invoke(context);
            }
        }
    }
}
