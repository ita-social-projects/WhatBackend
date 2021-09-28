using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/ApiError/{statusCode}/{message}")]
        public IActionResult ApiError(uint statusCode, string message)
        {
            var decodedMessage = HttpUtility.UrlDecode(message);

            ViewBag.statusCode = statusCode;
            ViewBag.message = decodedMessage;

            return View();
        }
    }
}
