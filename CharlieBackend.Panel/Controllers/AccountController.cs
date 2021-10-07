using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using CharlieBackend.Panel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;

namespace CharlieBackend.Panel.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IOptions<ApplicationSettings> _config;
        private readonly IApiUtil _apiUtil;
        private readonly IDataProtector _protector;
        private readonly AccountsApiEndpoints _accountsApiEndpoints;

        public AccountController(IOptions<ApplicationSettings> config, 
                                 IApiUtil apiUtil, 
                                 IDataProtectionProvider provider)
        {
            _apiUtil = apiUtil;
            _config = config;
            _accountsApiEndpoints = _config.Value.Urls.ApiEndpoints.Accounts;

            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticationDto authDto)
        {
            var responseModel = await _apiUtil.SignInAsync(_accountsApiEndpoints.SignIn, authDto);

            var token = responseModel.Token.Replace("Bearer ", "");
            
            if (token == null || !await Authenticate(token))
            {
                return RedirectToAction("Login", "Account");
            }

            Dictionary<string, string> roleList = new Dictionary<string, string>();

            foreach (var item in responseModel.RoleList)
            {
                string value = _protector.Protect(item.Value.Replace("Bearer ", ""));
                roleList.Add(item.Key, value);
            }

            SetResponseCookie("accessToken", _protector.Protect(token));

            TempData["authTokens"] = roleList;

            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangeRole()
        {
            Request.Cookies.TryGetValue("accessToken", out string token);
            token = _protector.Unprotect(token);
            
            await Authenticate(token);

            SetResponseCookie("accessToken", _protector.Protect(token));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        private async Task<bool> Authenticate(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            SetResponseCookie("currentRole", role);

            var claims = new List<Claim>
            {
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
           
            ClaimsIdentity roleClaim = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // set authentication cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(roleClaim));

            return true;
        }

        private void SetResponseCookie(string key, string value)
        {
            Response.Cookies.Append(key, value, new CookieOptions()
            {
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Secure = true
            });
        }
    }
}
