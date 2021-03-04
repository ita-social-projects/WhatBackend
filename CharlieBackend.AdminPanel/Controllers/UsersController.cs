using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IApiUtil _apiUtil;

        public UsersController(IApiUtil apiUtil)
        {
            _apiUtil = apiUtil;
        }

        [HttpGet]
        public async Task<ActionResult<IList<AccountDto>>> GetAllAccounts()
        {
            var accounts = await _apiUtil.GetAsync<IList<AccountDto>>($"api/accounts");

            return Ok(accounts);
        }

    }
}
