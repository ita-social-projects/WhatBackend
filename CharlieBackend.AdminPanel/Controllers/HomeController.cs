using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
            var decodeMessage = HttpUtility.UrlDecode(message);

            ViewBag.statusCode = statusCode;
            ViewBag.message = decodeMessage;

            return View();
        }
    }
}
