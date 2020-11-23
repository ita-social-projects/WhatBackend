using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.AdminPanel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class ThemesController : Controller
    {
        private readonly IThemeService _themeService;

        public ThemesController(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public async Task<IActionResult> AllThemes()
        {
            var themes = await _themeService.GetAllThemesAsync(Request.Cookies["accessToken"]);

            return View(themes);
        }

        public async Task<IActionResult> DeleteTheme(long id)
        {
            await _themeService.DeleteTheme(id, Request.Cookies["accessToken"]);

            return RedirectToAction("AllThemes", "Themes");
        }

    }
}
