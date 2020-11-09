using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CharlieBackend.AdminPanel.Models;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.DTO.Account;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("api/admin/account")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IApiUtil _apiUtil;

        public AccountController(ILogger<AccountController> logger, IOptions<ApplicationSettings> config, IApiUtil apiUtil)
        {
            _logger = logger;
            _apiUtil = apiUtil;

            _config = config;
        }

        [HttpGet("LogIn")]
        public async Task<IActionResult> LogIn()
        {
            var httpResponse = await _apiUtil.SignInAsync($"{_config.Value.Urls.Api.Https}/api/accounts/auth", new AuthenticationDto
            {
                Email = "admin.@gmail.com",
                Password = "admin"
            });


            HttpContext.Session.SetString("accessToken", httpResponse);

            Console.WriteLine("____________________ 1 : " + HttpContext.Session.GetString("accessToken"));

            return Ok(httpResponse);
            //return await httpResponse.Content.ReadAsStringAsync(); //get response body
            //return httpResponse.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(); // get auth token
        }


        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {

            return NoContent();
        }


    }
}
