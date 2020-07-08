using CharlieBackend.Api.Settings;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api")]
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

        [HttpPost("auth")]
        public async Task<ActionResult<AccountInfoModel>> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var foundAccount = await _accountService.GetAccountCredentialsAsync(authenticationModel);
            if (foundAccount == null) return Unauthorized("Incorrect credentials, please try again.");

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: new List<Claim> {
                      //  new Claim("Email", foundAccount.Email),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, foundAccount.Role.ToString())
                    },
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                FirstName = foundAccount.FirstName,
                LastName = foundAccount.LastName,
                Role = foundAccount.Role
            };
            Response.Headers.Add("Authorization", "Bearer " + encodedJwt);
            return Ok(response);
        }
    }
}
