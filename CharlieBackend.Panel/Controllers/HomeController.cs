using CharlieBackend.Panel.Models.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Web;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        public HomeController(IStringLocalizer<HomeController> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            if (Languages.language == Language.UA)
                Languages.language = Language.EN;
            else
                Languages.language = Language.UA;

            return View(_stringLocalizer);
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
