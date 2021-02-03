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
        public async Task InvokeAsync(HttpContext context, IAccountService accountService)
        {
            if (context.Request.Path.Value.Contains("accounts")
                || context.Request.Path.Value.Contains("swagger"))
            {
                await _next.Invoke(context);
            }
            else
            {
                string authHeader = context.Request.Headers["Authorization"];

                if (authHeader != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    authHeader = authHeader.ToString().Replace("Bearer ", "");

                    var jsonToken = handler.ReadToken(authHeader);
                    var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

                    var email = tokenS.Claims.First(claim => claim.Type == "Email").Value;

                    var isActive = await accountService.IsAccountActiveAsync(email);

                    if (isActive == null)
                    {
                        context.Response.StatusCode = 401;

                        await context.Response.WriteAsync("Need to sign in.");
                    }

                    if ((bool)!isActive)
                    {
                        context.Response.StatusCode = 403;

                        await context.Response.WriteAsync("Account is not active(i.e. you don't have proper role). You need to authorize!");
                    }

                    if (context.Request.Path.Value.Contains("lessons"))
                    {
                        var role = tokenS.Claims
                                .First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                        UserRole userRole = UserRoleExtension.ToEnum<UserRole>(role);

                        if (userRole == UserRole.Mentor)
                        {
                            var id = tokenS.Claims.First(claim => claim.Type == "Id").Value;

                            context.Items["mentorId"] = id;
                        }
                    }

                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 400;

                    await context.Response.WriteAsync("Bad token.");
                }
            }
        }
    }
}
