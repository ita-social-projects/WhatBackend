using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/ApiError/{statusCode}/{message}")]
        public IActionResult ApiError(uint statusCode, string message)
        {
            ViewBag.statusCode = statusCode;
            ViewBag.message = message;

            return View();
        }
    }
}
