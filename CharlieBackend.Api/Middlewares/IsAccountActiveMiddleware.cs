using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Api.Extensions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace CharlieBackend.Api.Middlewares
{
    /// <summary>
    /// Middleware that restricts requests from users with deactivated accounts.
    /// </summary>
    public class IsAccountActiveMiddleware
    {
        #region
        private readonly RequestDelegate _next;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public IsAccountActiveMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task InvokeAsync(HttpContext context, IAccountService accountService, ICurrentUserService currentUserService)
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
