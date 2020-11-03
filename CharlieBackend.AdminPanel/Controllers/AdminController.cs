using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CharlieBackend.AdminPanel.Models;
using CharlieBackend.AdminPanel.Utils;
using CharlieBackend.Core.Models.Account;
using CharlieBackend.AdminPanel.Utils.Interfaces;

namespace CharlieBackend.AdminPanel.Controllers
{
   // [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        private readonly IHttpUtil _httpUtil;

       
        public AdminController(ILogger<AdminController> logger, IHttpUtil httpUtil)
        {
            _logger = logger;
            _httpUtil = httpUtil;
        }

        [HttpGet]
        public async Task<ActionResult<object>> Test()
        {
            var httpRespone = await _httpUtil.PostJsonAsync("http://localhost:5000/api/auth", new AuthenticationModel
            {
                                                 Email = "Frodo.@gmail.com",
                                                 Password ="123456"
                                               });

            return await httpRespone.Content.ReadAsStringAsync(); //get response body
            //return httpRespone.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(); // get auth token
        }

        [HttpGet]
        public async Task<ActionResult<object>> Test2()
        {
            var x = await _httpUtil.GetAsync("https://localhost:5001/api/themes");

            return x.Content;
        }

        public IActionResult Index()
        {
            return View();
        }
               
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
