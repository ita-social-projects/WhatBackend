using CharlieBackend.Panel.Models.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Web;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (Languages.language == Language.UA)
                Languages.language = Language.EN;
            else
                Languages.language = Language.UA;


            ViewData["Wellcome"] = _stringLocalizer["Wellcome"].Value;
            ViewData["Greeting"] = _stringLocalizer["Greeting"].Value;
            ViewData["Description"] = _stringLocalizer["Description"].Value;

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
