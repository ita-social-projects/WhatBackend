using CharlieBackend.Api.SwaggerExamples.AccountsController;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manipulate telegram functions
    /// </summary>
    [Route("api/v{version:apiVersion}/telegram")]
    [ApiVersion("2.0")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        #region private readonly variables
        private readonly ITelegramService _telegramService;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        private readonly ISecretaryService _secretaryService;
        private readonly IJwtGenerator _jWTGenerator;
        #endregion

        /// <summary>
        /// Telegram controller constructor
        /// </summary>
        public TelegramController(ITelegramService telegramService,
            IMentorService mentorService,
            IStudentService studentService,
            ISecretaryService secretaryService,
            IJwtGenerator jWTGenerator
            )
        {
            _telegramService = telegramService;
            _studentService = studentService;
            _mentorService = mentorService;
            _secretaryService = secretaryService;
            _jWTGenerator = jWTGenerator;
        }

        /// <summary>
        /// Get link for Telegram account synchronization
        /// </summary>
        /// <returns>URL</returns>
        //[SwaggerResponse(200, type: typeof(SignInResponse))]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpGet]
        public async Task<ActionResult<string>> GetTelegramBotLink()
        {
            var link = await _telegramService.GetTelegramBotLink();

            return link.ToActionResult();
        }

        /// <summary>
        /// Syncronized telegram account with user account from database
        /// </summary>
        /// <response code="200">Successful return an updated account entity</response>
        [HttpPost("sync/{telegramToken}/{telegramId}")]
        public async Task<ActionResult<Account>> AccountSync(string telegramToken, string telegramId)
        {
            var syncronizedAccount = await _telegramService.SynchronizeTelegramAccount(telegramToken, telegramId);

            return syncronizedAccount.ToActionResult();
        }

        /// <summary>
        /// Allows user to sign in
        /// </summary>
        /// <returns>JWT</returns>
        /// <response code="200">User successfully logged in</response>
        /// <response code="HTTP: 400, API: 0">Impossible to log in</response>
        /// <response code="HTTP: 401, API: 1">Impossible to log in due to wrong credentials</response>
        /// <response code="HTTP: 401, API: 1">Account is not active</response>
        /// <response code="403">Account not approved</response>
        [SwaggerResponse(200, type: typeof(SignInResponse))]
        [SwaggerResponseHeader(200, "Authorization Bearer", "string", "token")]
        [Route("auth")]
        [HttpPost]
        public async Task<ActionResult> SignInWithTelegramId([FromBody] string telegramId)
        {
            var foundAccount = (await _telegramService.GetAccountByTelegramIdAsync(telegramId)).Data;
            var roleIds = new Dictionary<UserRole, long>();

            #region ValidationChecks

            if (foundAccount == null)
            {
                return Unauthorized("Incorrect credentials, please try again.");
            }

            if (!foundAccount.IsActive)
            {
                return StatusCode(401, "Account is not active!");
            }

            if (foundAccount.Role.IsNotAssigned())
            {
                return StatusCode(403, $"{foundAccount.Email}  is registered and waiting assign.");
            }

            if (foundAccount.Role.Is(UserRole.Student))
            {
                var foundStudent = (await _studentService.GetStudentByAccountIdAsync(foundAccount.Id)).Data;

                if (foundStudent == null)
                {
                    return BadRequest();
                }
                else
                    roleIds.Add(UserRole.Student, foundStudent.Id);
            }

            if (foundAccount.Role.Is(UserRole.Mentor))
            {
                var foundMentor = (await _mentorService.GetMentorByAccountIdAsync(foundAccount.Id)).Data;

                if (foundMentor == null)
                {
                    return BadRequest();
                }
                else
                    roleIds.Add(UserRole.Mentor, foundMentor.Id);
            }

            if (foundAccount.Role.Is(UserRole.Secretary))
            {
                var foundSecretary = (await _secretaryService.GetSecretaryByAccountIdAsync(foundAccount.Id)).Data;

                if (foundSecretary == null)
                {
                    return BadRequest();
                }
                else
                    roleIds.Add(UserRole.Secretary, foundSecretary.Id);
            }

            if (foundAccount.Role.IsAdmin())
            {
                roleIds.Add(UserRole.Admin, foundAccount.Id);
            }

            #endregion

            Dictionary<string, string> userRoleToJwtToken = _jWTGenerator.GetRoleJwtDictionary  (foundAccount, roleIds);

            var response = new AuthenticationResponseDto
            {
                FirstName = foundAccount.FirstName,
                LastName = foundAccount.LastName,
                Role = foundAccount.Role,
                RoleList = userRoleToJwtToken,
                Localization = foundAccount.Localization
            };

            GetHeaders(userRoleToJwtToken);

            return Ok(response);
        }

        /// <summary>
        /// Deletes expired Telegram tokens from database
        /// </summary>
        /// <returns>true</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("clear")]
        public async Task<bool> ClearOldTelegramTokens()
        {
            return await _telegramService.ClearOldTelegramTokens();
        }

        /// <summary>
        /// Deletes TelegramId of user from database
        /// </summary>
        /// <returns>true</returns>
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpDelete("delete")]
        public async Task<bool> DeleteAccoutsSyncronization()
        {
            return await _telegramService.ClearOldTelegramTokens();
        }

        private void GetHeaders(Dictionary<string, string> tokenDictionary)
        {
            if (tokenDictionary.ContainsKey(UserRole.Admin.ToString()))
            {
                Response.Headers.Add("Authorization", tokenDictionary[UserRole.Admin.ToString()]);
            }
            else if (tokenDictionary.ContainsKey(UserRole.Secretary.ToString()))
            {
                Response.Headers.Add("Authorization", tokenDictionary[UserRole.Secretary.ToString()]);
            }
            else if (tokenDictionary.ContainsKey(UserRole.Mentor.ToString()))
            {
                Response.Headers.Add("Authorization", tokenDictionary[UserRole.Mentor.ToString()]);
            }
            else
            {
                Response.Headers.Add("Authorization", tokenDictionary[UserRole.Student.ToString()]);
            }

            Response.Headers.Add("Access-Control-Expose-Headers", "x-token, Authorization");
        }
    }
}
