using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{

    [Authorize(Roles = "Admin, Secretary")]
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
