using CharlieBackend.Panel.Models.Languages;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Web;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _stringLocalizer;
        private readonly ICurrentUserService _currentUserService;

        public HomeController(IStringLocalizer<HomeController> stringLocalizer, ICurrentUserService currentUserService)
        {
            _stringLocalizer = stringLocalizer;
            _currentUserService = currentUserService;
        }

        public IActionResult Index()
        {
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

        public IActionResult ChangeLanguage(Language lang)
        {
            if (!_currentUserService.Localization.Equals(lang.ToDescriptionString()))
                _currentUserService.Localization = lang.ToDescriptionString();

            return RedirectToAction("Index");
        }
    }
}
