using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CharlieBackend.AdminPanel.Models;
using CharlieBackend.Core.Models.Account;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Microsoft.Extensions.Options;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IOptions<ApplicationSettings> _config;

        private readonly IHttpUtil _httpUtil;

       
        public AdminController(ILogger<AdminController> logger, IOptions<ApplicationSettings> config, IHttpUtil httpUtil)
        {
            _logger = logger;
            _httpUtil = httpUtil;
            _config = config;
        }

        [HttpGet("Test")]
        public async Task<ActionResult<object>> Test()
        {
            var httpRespone = await _httpUtil.PostJsonAsync( $"{_config.Value.Urls.Api.Https}/api/auth", new AuthenticationModel 
            {
                                                 Email = "Frodo.@gmail.com",
                                                 Password ="123456"
                                               });

            return await httpRespone.Content.ReadAsStringAsync(); //get response body
            //return httpRespone.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(); // get auth token
        }

        [HttpGet("Test2")]
        public async Task<ActionResult<object>> Test2()
        {
            var x = await _httpUtil.GetAsync($"{_config.Value.Urls.Api.Https}/api/themes");

            return x.Content;
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
