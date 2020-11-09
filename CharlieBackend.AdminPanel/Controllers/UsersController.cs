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

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("api/admin")]
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

        [HttpGet("signin")]
        public async Task<ActionResult<string>> SignIn()
        {
            var httpResponse = await _apiUtil.SignInAsync($"{_config.Value.Urls.Api.Https}/api/accounts/auth", new AuthenticationDto
            {
                Email = "Frodo.@gmail.com",
                Password = "123456"
            });

            HttpContext.Session.SetString("accessToken", httpResponse);

            Console.WriteLine("____________________ 1 : " + HttpContext.Session.GetString("accessToken"));

            return Ok(httpResponse);
            //return await httpResponse.Content.ReadAsStringAsync(); //get response body
            //return httpResponse.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(); // get auth token
        }



        [HttpGet("Test2")]
        public async Task<ActionResult<IList<ThemeDto>>> Test2()
        {
            Console.WriteLine("____________________ 2 : " + HttpContext.Session.GetString("accessToken"));

            var x = await _apiUtil.GetAsync<IList<ThemeDto>>($"{_config.Value.Urls.Api.Https}/api/themes", HttpContext.Session.GetString("accessToken"));

            return Ok(x);
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
