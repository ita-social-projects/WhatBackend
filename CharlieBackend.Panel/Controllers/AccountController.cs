using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Panel.Helpers;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var accountId = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.AccountId).Value;
            var entityId = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.EntityId).Value;
            var email = tokenS.Claims.FirstOrDefault(claim => claim.Type == ClaimsConstants.Email).Value;

            SetResponseCookie("currentRole", role);
            SetResponseCookie("accountId", accountId);
            SetResponseCookie("entityId", entityId);
            SetResponseCookie("email", email);

            var claims = new List<Claim>
            {
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                 new Claim(ClaimsConstants.AccountId, accountId),
                 new Claim(ClaimsConstants.EntityId, entityId),
                 new Claim(ClaimsConstants.Email, email)
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
