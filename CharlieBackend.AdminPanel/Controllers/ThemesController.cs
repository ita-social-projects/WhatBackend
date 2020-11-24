using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Models.Theme;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateTheme(long id)
        {
            var themes = await _themeService.GetAllThemesAsync(Request.Cookies["accessToken"]);
            ViewBag.Theme = themes.First(el => el.Id == id);
            return View("UpdateTheme");
        }

        [HttpPost]
        public async Task<IActionResult> AddTheme(CreateThemeDto themeDto)
        {
            await _themeService.AddThemeAsync(themeDto, Request.Cookies["accessToken"]);

            return RedirectToAction("AllThemes", "Themes");
        }

        [HttpPost("{id}")]  
        public async Task<IActionResult> UpdateTheme(long id, UpdateThemeDto data)
        {
            await _themeService.UpdateTheme(id, data, Request.Cookies["accessToken"]);

            return RedirectToAction("AllThemes", "Themes");
        }

        public async Task<IActionResult> DeleteTheme(long id)
        {
            await _themeService.DeleteTheme(id, Request.Cookies["accessToken"]);

            return RedirectToAction("AllThemes", "Themes");
        }

    }
}
