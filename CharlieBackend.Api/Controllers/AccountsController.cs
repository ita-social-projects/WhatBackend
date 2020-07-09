using CharlieBackend.Api.Settings;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //[HttpPost("account")]
        //public async Task<ActionResult<AccountModel>> PostAccount(AccountModel accountModel)
        //{
        //    if (!ModelState.IsValid) return BadRequest();

        //    var authenticationModel = new AuthenticationModel { email = accountModel.Email, password = accountModel.Password };
        //    var existingAccount = await _accountService.GetAccountCredentialsAsync(authenticationModel);
        //    if (existingAccount != null) return StatusCode(409, "Account already exists!");

        //    var createdAccount = await _accountService.CreateAccountAsync(accountModel);
        //    if (createdAccount == null) return StatusCode(500);

        //    return Ok();
        //}

        [HttpPost]
        public async Task<ActionResult<AccountInfoModel>> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var foundAccount = await _accountService.GetAccountCredentialsAsync(authenticationModel);
            if (foundAccount == null) return Unauthorized("Incorrect credentials, please try again.");

            if (!foundAccount.IsActive) return StatusCode(401, "Account is not active!");

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: new List<Claim> {
                      //  new Claim("Email", foundAccount.Email),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, foundAccount.Role.ToString()),
                        new Claim("Email", foundAccount.Email)
                    },
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                first_Name = foundAccount.FirstName,
                last_Name = foundAccount.LastName,
                role = foundAccount.Role
            };
            Response.Headers.Add("Authorization", "Bearer " + encodedJwt);
            return Ok(response);
        }

        [Authorize(Roles = "1, 2, 4")]
        [HttpDelete]
        public async Task<ActionResult> DisableAccount()
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                var authHeader = Request.Headers[HeaderNames.Authorization];
                authHeader = authHeader.ToString().Replace("Bearer ", "");

                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

                var role = tokenS.Claims.First(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
                var email = tokenS.Claims.First(claim => claim.Type == "Email").Value;

                var isDisabled = await _accountService.DisableAccountAsync(email);
                if (isDisabled) return NoContent();
                return StatusCode(500, "Error occurred while trying to disable accout");

            } catch { return StatusCode(401, "Bad token."); }
        }

    }
}
