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
    /// 
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

                    if (isActive == null)
                    {
                        context.Response.StatusCode = 401;

                        await context.Response.WriteAsync("Need to sign in.");
                    }

                    if ((bool)!isActive)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;

                        await context.Response.WriteAsync("Account is not active(i.e. you don't have proper role). You need to authorize!");
                    }

                    if (context.Request.Path.Value.Contains("lessons"))
                    {
                        var role = currentUserService.Role;

                        if (role == UserRole.Mentor)
                        {
                            var id = currentUserService.EntityId;

                            context.Items["mentorId"] = id;
                        }
                    }

                    await _next.Invoke(context);
            }
        }
    }
}
