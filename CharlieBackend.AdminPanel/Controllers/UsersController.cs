using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("api/admin/users")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IApiUtil _apiUtil;


        public UsersController(ILogger<UsersController> logger, IOptions<ApplicationSettings> config, IApiUtil apiUtil)
        {
            _logger = logger;
            _apiUtil = apiUtil;

            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IList<AccountDto>>> GetAllAccounts()
        {
            var accounts = await _apiUtil.GetAsync<IList<AccountDto>>($"{_config.Value.Urls.Api.Https}/api/accounts", HttpContext.Session.GetString("accessToken"));

            return Ok(accounts);
        }

    }
}
