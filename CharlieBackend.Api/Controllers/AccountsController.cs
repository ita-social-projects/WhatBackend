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
    /// Controller to manupulate with account
    /// </summary>
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        #region
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;
        private readonly IMentorService _mentorService;
        private readonly ISecretaryService _secretaryService;
        private readonly IJwtGenerator _jWTGenerator;
        #endregion
        /// <summary>
        /// Account controller constructor
        /// </summary>
        public AccountsController(IAccountService accountService,
                IStudentService studentService,
                IMentorService mentorService,
                ISecretaryService secretaryService,
                IJwtGenerator jWTGenerator)
        {
            _accountService = accountService;
            _studentService = studentService;
            _mentorService = mentorService;
            _secretaryService = secretaryService;
            _jWTGenerator = jWTGenerator;
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
        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<ActionResult> SignIn20([FromBody] AuthenticationDto authenticationModel)
        {
            var foundAccount = (await _accountService.GetAccountCredentialsAsync(authenticationModel)).Data;
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
                return StatusCode(403, foundAccount.Email + " is registered and waiting assign.");
            }

            if (foundAccount.Role.Is(UserRole.Student))
            {
                var foundStudent = (await _studentService.GetStudentByAccountIdAsync(foundAccount.Id)).Data;

                if (foundStudent == null)
                {
                    return BadRequest();
                }
                else roleIds.Add(UserRole.Student, foundStudent.Id);
            }

            if (foundAccount.Role.Is(UserRole.Mentor))
            {
                var foundMentor = (await _mentorService.GetMentorByAccountIdAsync(foundAccount.Id)).Data;

                if (foundMentor == null)
                {
                    return BadRequest();
                }
                else roleIds.Add(UserRole.Mentor, foundMentor.Id);
            }

            if (foundAccount.Role.Is(UserRole.Secretary))
            {
                var foundSecretary = (await _secretaryService.GetSecretaryByAccountIdAsync(foundAccount.Id)).Data;

                if (foundSecretary == null)
                {
                    return BadRequest();
                }
                else roleIds.Add(UserRole.Secretary, foundSecretary.Id);
            }

            if (foundAccount.Role.IsAdmin())
            {
                roleIds.Add(UserRole.Admin, foundAccount.Id);
            }

            #endregion

            Dictionary<string, string> userRoleToJwtToken = _jWTGenerator.GetRoleJwtDictionary(foundAccount, roleIds);

            var response = new
            {
                first_name = foundAccount.FirstName,
                last_name = foundAccount.LastName,
                role = foundAccount.Role,
                roleList = userRoleToJwtToken
            };
            GetHeaders(userRoleToJwtToken);

            return Ok(response);
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
        [MapToApiVersion("1.0")]
        [Route("auth")]
        [HttpPost]
        public async Task<object> SignIn([FromBody] AuthenticationDto authenticationModel)
        {
            var foundAccount = (await _accountService.GetAccountCredentialsAsync(authenticationModel)).Data;
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

            long entityId = foundAccount.Id;

            if (foundAccount.Role.IsNotAssigned())
            {
                return StatusCode(403, foundAccount.Email + " is registered and waiting assign.");
            }

            if (foundAccount.Role.Is(UserRole.Student))
            {
                var foundStudent = (await _studentService.GetStudentByAccountIdAsync(foundAccount.Id)).Data;

                if (foundStudent == null)
                {
                    return BadRequest();
                }
                else roleIds.Add(UserRole.Student, foundStudent.Id);

                entityId = foundStudent.Id;
            }

            if (foundAccount.Role.Is(UserRole.Mentor))
            {
                var foundMentor = (await _mentorService.GetMentorByAccountIdAsync(foundAccount.Id)).Data;

                if (foundMentor == null)
                {
                    return BadRequest();
                }
                else roleIds.Add(UserRole.Mentor, foundMentor.Id);

                entityId = foundMentor.Id;
            }

            if (foundAccount.Role.Is(UserRole.Secretary))
            {
                var foundSecretary = (await _secretaryService.GetSecretaryByAccountIdAsync(foundAccount.Id)).Data;

                if (foundSecretary == null)
                {
                    return BadRequest();
                }
                else roleIds.Add(UserRole.Secretary, foundSecretary.Id);

                entityId = foundSecretary.Id;
            }

            if (foundAccount.Role.IsAdmin())
            {
                roleIds.Add(UserRole.Admin, foundAccount.Id);

                entityId = foundAccount.Id;
            }

            #endregion

            Dictionary<string, string> userRoleToJwtToken = _jWTGenerator.GetRoleJwtDictionary(foundAccount, roleIds);

            var response = new
            {
                first_name = foundAccount.FirstName,
                last_name = foundAccount.LastName,
                role = foundAccount.Role,
                id = entityId
            };
            GetHeaders(userRoleToJwtToken);

            return Ok(response);
        }

        /// <summary>
        /// Give role to account
        /// </summary>
        /// <response code="200">Role successfully given</response>
        /// <response code="HTTP: 404, API: 3">Account not found</response>
        /// <response code="HTTP: 409, API: 5">Account already has this role
        /// or role is unsuitable</response>
        [Authorize(Roles = "Admin")]
        [MapToApiVersion("2.0")]
        [Route("role/grant")]
        [HttpPut]
        public async Task<ActionResult> GrantRoleToAccount(AccountRoleDto account)
        {
            var changeAccountRoleModel = await _accountService
                    .GrantRoleToAccount(account);

            return changeAccountRoleModel.ToActionResult();
        }

        /// <summary>
        /// Remove role from account
        /// </summary>
        /// <response code="200">Role successfully given</response>
        /// <response code="HTTP: 404, API: 3">Account not found</response>
        /// <response code="HTTP: 409, API: 5">Account doesn't have this role
        /// or role is unsuitable</response>
        [Authorize(Roles = "Admin")]
        [MapToApiVersion("2.0")]
        [Route("role/revoke")]
        [HttpPut]
        public async Task<ActionResult> RevokeRoleFromAccount(AccountRoleDto account)
        {
            var changeAccountRoleModel = await _accountService
                    .RevokeRoleFromAccount(account);

            return changeAccountRoleModel.ToActionResult();
        }

        /// <summary>
        /// Registration of account
        /// </summary>
        /// <response code="200">User successfully registered</response>
        /// <response code="HTTP: 409, API: 5">Email already exists</response>
        [SwaggerResponse(200, type: typeof(AccountDto))]
        [Route("reg")]
        [HttpPost]
        public async Task<ActionResult> PostAccount(CreateAccountDto accountModel)
        {
            var createdAccountModel = await _accountService.CreateAccountAsync(accountModel);

            return createdAccountModel.ToActionResult();
        }

        /// <summary>
        /// Returns all registered accounts
        /// </summary>
        /// <response code="200">Successful return of list of registered accounts</response>
        [SwaggerResponse(200, type: typeof(IList<AccountDto>))]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllAccount()
        {
            return Ok(await _accountService.GetAllAccountsAsync());
        }

        /// <summary>
        /// Returns all not assigned accounts
        /// </summary>
        /// <response code="200">Successful return of list of all accounts which is not assigned to any role entity</response>
        [Route("NotAssigned")]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet]
        public async Task<ActionResult<List<AccountDto>>> GetAllNotAssignedAccounts()
        {

            var accountsModels = await _accountService.GetAllNotAssignedAccountsAsync();

            return Ok(accountsModels);
        }

        /// <summary>
        /// Returns an updated account
        /// </summary>
        /// <response code="200">Successful return an updated account entity</response>
        [Route("password")]
        [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
        [HttpPut]
        public async Task<ActionResult> ChangePassword(ChangeCurrentPasswordDto changePassword)
        {
            var updatedAccount = await _accountService.ChangePasswordAsync(changePassword);

            return updatedAccount.ToActionResult();
        }

        /// <summary>
        /// Returns a result of generating a forgot password token
        /// </summary>
        /// <response code="200">Successful returns a forgot password DTO</response>
        [Route("password/forgot")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto changeForgotPassword)
        {
            var generatedForgotTokenResult = await _accountService.GenerateForgotPasswordToken(changeForgotPassword);

            return generatedForgotTokenResult.ToActionResult();
        }

        /// <summary>
        /// Returns a result of confirmed password change
        /// </summary>
        /// <response code="200">Successful return an updated account entity</response>
        [Route("password/reset/{guid}")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ResetForgotPassword(string guid, ResetPasswordDto resetPassword)
        {
            var updatedAccount = await _accountService.ResetPasswordAsync(guid, resetPassword);

            return updatedAccount.ToActionResult();
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
            else Response.Headers.Add("Authorization", tokenDictionary[UserRole.Student.ToString()]);

            Response.Headers.Add("Access-Control-Expose-Headers", "x-token, Authorization");
        }
    }
}
