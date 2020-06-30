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


        [HttpPost("account")]
        public async Task<ActionResult<AccountModel>> PostAccount(AccountModel accountModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var createdAccount = await _accountService.CreateAccountAsync(accountModel);
            if (createdAccount == null) return StatusCode(500);

            return Ok();
        }

        [HttpPost("auth")]
        public async Task<ActionResult<AccountModel>> GetAccountCredentials(AuthenticationModel authenticationModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                // TODO: do not send unwanted properties
                var foundAccount = await _accountService.GetAccountCredentials(authenticationModel);
                if (foundAccount != null) return Ok(foundAccount);
                else return Unauthorized();
            }
            catch { return StatusCode(500); }
        }
    }
}
