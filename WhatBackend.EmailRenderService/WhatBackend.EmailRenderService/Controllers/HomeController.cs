using Microsoft.AspNetCore.Mvc;

namespace WhatBackend.EmailRenderService.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("EmailRenderService");
    }
}
