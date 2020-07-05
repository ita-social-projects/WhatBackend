using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.Models.Account;
using Microsoft.AspNetCore.Mvc;
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

            if (foundAccount != null) return Ok(foundAccount.ToUserModel());
            else return Unauthorized("Incorrect credentials, please try again.");
        }
    }
}
