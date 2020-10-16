using CharlieBackend.Business.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace CharlieBackend.Api.Middlewares
{
    public class IsAccountActiveMiddleware
    {
        #region
        private readonly RequestDelegate _next;
        #endregion

        public IsAccountActiveMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccountService accountService)
        {
            if (context.Request.Path.Value.Contains("auth")
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
                        context.Response.StatusCode = 401;

                        await context.Response.WriteAsync("Account is not active!");
                    }

                    if (context.Request.Path.Value.Contains("lessons"))
                    {
                        var role = tokenS.Claims
                                .First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

                        if (role == "2")
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
