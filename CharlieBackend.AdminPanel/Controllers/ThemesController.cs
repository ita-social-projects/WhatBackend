using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateTheme(long id)
        {
            var themes = await _themeService.GetAllThemesAsync(_protector.Unprotect(Request.Cookies["accessToken"]));
            ViewBag.Theme = themes.First(el => el.Id == id);

            return View("UpdateTheme");
        }

        [HttpPost]
        public async Task<IActionResult> AddTheme(CreateThemeDto themeDto)
        {
            await _themeService.AddThemeAsync(themeDto, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllThemes", "Themes");
        }

        [HttpPost("{id}")]  
        public async Task<IActionResult> UpdateTheme(long id, UpdateThemeDto data)
        {
            await _themeService.UpdateTheme(id, data, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllThemes", "Themes");
        }

        public async Task<IActionResult> DeleteTheme(long id)
        {
            await _themeService.DeleteTheme(id, _protector.Unprotect(Request.Cookies["accessToken"]));

            return RedirectToAction("AllThemes", "Themes");
        }

    }
}
