using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.Models;
using CharlieBackend.Business.Services.Interfaces;

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
        public async Task<ActionResult<AccountModel>> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            if (!ModelState.IsValid) return BadRequest();
   
            var foundAccount = await _accountService.GetAccountCredentialsAsync(authenticationModel);

            // TODO: do not send anonymous type
            if (foundAccount != null) return Ok(new { foundAccount.Id, foundAccount.FirstName, foundAccount.LastName, foundAccount.Role });
            else return Unauthorized("Incorrect credentials, please try again.");
        }
    }
}
