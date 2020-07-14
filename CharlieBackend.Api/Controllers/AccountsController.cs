using CharlieBackend.Business.Options;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        private readonly AuthOptions _authOptions;

        public AccountsController(IAccountService accountService, IStudentService studentService, IMentorService mentorService, IOptions<AuthOptions> authOptions)
        {
            _accountService = accountService;
            _studentService = studentService;
            _mentorService = mentorService;
            _authOptions = authOptions.Value;
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(AuthenticationModel authenticationModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var foundAccount = await _accountService.GetAccountCredentialsAsync(authenticationModel);
            if (foundAccount == null) return Unauthorized("Incorrect credentials, please try again.");

            if (!foundAccount.IsActive) return StatusCode(401, "Account is not active!");

            long studentOrMentorId = default;
            if (foundAccount.Role == 1)
            {
                var foundStudent = await _studentService.GetStudentByAccountIdAsync(foundAccount.Id);
                if (foundStudent == null) return BadRequest();
                studentOrMentorId = foundStudent.Id;
            }
            else if (foundAccount.Role == 2)
            {
                var foundMentor = await _mentorService.GetMentorByAccountIdAsync(foundAccount.Id);
                if (foundMentor == null) return BadRequest();
                studentOrMentorId = foundMentor.Id;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.ISSUER,
                    audience: _authOptions.AUDIENCE,
                    notBefore: now,
                    claims: new List<Claim> {
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, foundAccount.Role.ToString()),
                        new Claim("Id", studentOrMentorId.ToString()),
                        new Claim("Email", foundAccount.Email)
                    },
                    expires: now.Add(TimeSpan.FromMinutes(_authOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                first_name = foundAccount.FirstName,
                last_name = foundAccount.LastName,
                role = foundAccount.Role,
                id = studentOrMentorId
            };
            Response.Headers.Add("Authorization", "Bearer " + encodedJwt);
            Response.Headers.Add("Access-Control-Expose-Headers", "x-token, Authorization");
            return Ok(response);
        }
    }
}
