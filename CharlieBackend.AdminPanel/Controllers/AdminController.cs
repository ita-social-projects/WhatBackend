using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CharlieBackend.AdminPanel.Models;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using CharlieBackend.Core.DTO.Theme;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Account;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IApiUtil _apiUtil;


        public AdminController(ILogger<AdminController> logger, IOptions<ApplicationSettings> config, IApiUtil apiUtil)
        {
            _logger = logger;
            _apiUtil = apiUtil;

            _config = config;
        }

        [HttpGet("signin")]
        public async Task<ActionResult<AuthenticationDto>> SignIn()
        {
            var httpResponse = await _apiUtil.SignInAsync<AuthenticationDto>($"{_config.Value.Urls.Api.Https}/api/auth", new AuthenticationDto
            {
                Email = "Frodo.@gmail.com",
                Password = "123456"
            });


            return httpResponse;
            //return await httpResponse.Content.ReadAsStringAsync(); //get response body
            //return httpResponse.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(); // get auth token
        }


        [HttpGet("Test2")]
        public async Task<ActionResult<IList<ThemeDto>>> Test2()
        {
            var x = await _apiUtil.GetAsync<IList<ThemeDto>>($"{_config.Value.Urls.Api.Https}/api/themes", null);

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
