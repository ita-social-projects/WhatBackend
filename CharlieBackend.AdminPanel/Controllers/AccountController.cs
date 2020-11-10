using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("api/admin/account")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IApiUtil _apiUtil;

        public AccountController(ILogger<AccountController> logger, IOptions<ApplicationSettings> config, IApiUtil apiUtil)
        {
            _logger = logger;
            _apiUtil = apiUtil;

            _config = config;
        }

        [HttpGet("LogIn")]
        public async Task<IActionResult> LogIn()
        {
            var httpResponseToken = await _apiUtil.SignInAsync($"{_config.Value.Urls.Api.Https}/api/accounts/auth", new AuthenticationDto
            {
                Email = "admin.@gmail.com",
                Password = "admin"
            });

            await Authenticate(httpResponseToken);

            HttpContext.Session.SetString("accessToken", httpResponseToken);

            return Ok(httpResponseToken);
        }


        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {

            return NoContent();
        }

        private async Task Authenticate(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            token = token.ToString().Replace("Bearer ", "");

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            var claims = new List<Claim>
            {
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
           
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // set authentication cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}
