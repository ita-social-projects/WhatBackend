using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CharlieBackend.Business.Options;
using System.IdentityModel.Tokens.Jwt;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        #region
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        private readonly ISecretaryService _secretaryService;
        private readonly AuthOptions _authOptions;
        #endregion

        public AccountsController(IAccountService accountService,
                IStudentService studentService,
                IMentorService mentorService,
                ISecretaryService secretaryService,
                IOptions<AuthOptions> authOptions)
        {
            _accountService = accountService;
            _studentService = studentService;
            _mentorService = mentorService;
            _secretaryService = secretaryService;
            _authOptions = authOptions.Value;
        }

        [Route("auth")]
        [HttpPost]
        public async Task<ActionResult> SignIn(AuthenticationDto authenticationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var foundAccount = await _accountService.GetAccountCredentialsAsync(authenticationModel);

            if (foundAccount == null)
            {
                return Unauthorized("Incorrect credentials, please try again.");
            }

            if (!foundAccount.IsActive)
            {
                return StatusCode(401, "Account is not active!");
            }

            long entityId = foundAccount.Id;

            if (foundAccount.Role == UserRole.Student)
            {
                var foundStudent = await _studentService.GetStudentByAccountIdAsync(foundAccount.Id);

                if (foundStudent == null)
                {
                    return BadRequest();
                }

                entityId = foundStudent.Id;
            }

            if (foundAccount.Role == UserRole.Mentor)
            {
                var foundMentor = await _mentorService.GetMentorByAccountIdAsync(foundAccount.Id);

                if (foundMentor == null)
                {
                    return BadRequest();
                }

                entityId = foundMentor.Id;
            }

            if (foundAccount.Role == UserRole.Secretary)
            {
                var foundSecretary = await _secretaryService.GetSecretaryByAccountIdAsync(foundAccount.Id);

                if (foundSecretary == null)
                {
                    return BadRequest();
                }

                entityId = foundSecretary.Data.Id;
            }

            if (foundAccount.Role == UserRole.NotAssigned)
            {
                return StatusCode(403, foundAccount.Email + " is registered and waiting assign.");
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.ISSUER,
                    audience: _authOptions.AUDIENCE,
                    notBefore: now,
                    claims: new List<Claim>
                    {
                            new Claim(ClaimsIdentity.DefaultRoleClaimType,
                                    foundAccount.Role.ToString()),
                            new Claim("Id", entityId.ToString()),
                            new Claim("Email", foundAccount.Email)
                    },
                    expires: now.Add(TimeSpan.FromMinutes(_authOptions.LIFETIME)),
                    signingCredentials:
                            new SigningCredentials(
                                    _authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                first_name = foundAccount.FirstName,
                last_name = foundAccount.LastName,
                role = foundAccount.Role,
                id = entityId
            };

            Response.Headers.Add("Authorization", "Bearer " + encodedJwt);
            Response.Headers.Add("Access-Control-Expose-Headers",
                    "x-token, Authorization"
                    );

            return Ok(response);
        }

        [Route("reg")]
        [HttpPost]
        public async Task<ActionResult> PostAccount(CreateAccountDto accountModel)
        {
            var createdAccountModel = await _accountService.CreateAccountAsync(accountModel);

            return createdAccountModel.ToActionResult();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllAccount()
        {
            return Ok(await _accountService.GetAllAccountsAsync());
        }
        
        [Route("NotAssigned")]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet]
        public async Task<ActionResult<List<AccountDto>>> GetAllNotAssignedAccounts()
        {

            var accountsModels = await _accountService.GetAllNotAssignedAccountsAsync();

            return Ok(accountsModels);
        }
    }
}
