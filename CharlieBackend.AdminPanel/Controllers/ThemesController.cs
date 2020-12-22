using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class ThemesController : Controller
    {
        private readonly IThemeService _themeService;

        private readonly IDataProtector _protector;

        private readonly IOptions<ApplicationSettings> _config;

        public ThemesController(IThemeService themeService, 
                                IDataProtectionProvider provider,
                                IOptions<ApplicationSettings> config)
        {
            _themeService = themeService;
            _config = config;
            _protector = provider.CreateProtector(_config.Value.Cookies.SecureKey);
        }

        public async Task<IActionResult> AllThemes()
        {
            var themes = await _themeService.GetAllThemesAsync(_protector.Unprotect(Request.Cookies["accessToken"]));

            return View(themes);
        }

        [HttpPost]
        public async Task<IActionResult> AddTheme(CreateThemeDto themeDto)
        {
            await _themeService.AddThemeAsync(themeDto, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllThemes", "Themes");
        }

        [HttpGet("{id}")]  
        public async Task<IActionResult> UpdateTheme(long id, UpdateThemeDto updateThemeDto)
        {
            await _themeService.UpdateTheme(id, updateThemeDto, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllThemes", "Themes");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteTheme(long id)
        {
            await _themeService.DeleteTheme(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllThemes", "Themes");
        }

    }
}
