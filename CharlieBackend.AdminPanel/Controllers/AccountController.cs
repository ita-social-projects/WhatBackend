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
    [Route("admin/account")]
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

        [HttpGet("Login")]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AuthenticationDto authDto)
        {
            var httpResponseToken = await _apiUtil.SignInAsync($"{_config.Value.Urls.Api.Https}/api/accounts/auth", authDto);

            if(httpResponseToken == null || !await AuthenticateAdmin(httpResponseToken))
            {
                return RedirectToAction("Login", "Account");
            }

            Response.Cookies.Append("accessToken", httpResponseToken);


            return RedirectToAction("Index", "Home");
        }


        [HttpGet("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        private async Task<bool> AuthenticateAdmin(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            token = token.ToString().Replace("Bearer ", "");

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            if(role != "Admin")
            {
                return false;
            }

            var claims = new List<Claim>
            {
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
           
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // set authentication cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

            return true;
        }

    }
}
