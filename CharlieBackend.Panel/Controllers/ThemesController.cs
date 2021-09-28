using System.Threading.Tasks;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
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
            var themes = await _themeService.GetAllThemesAsync();

            return View(themes);
        }

        [HttpPost]
        public async Task<IActionResult> AddTheme(CreateThemeDto themeDto)
        {
            await _themeService.AddThemeAsync(themeDto);

            return RedirectToAction("AllThemes", "Themes");
        }

        [HttpGet("{id}")]  
        public async Task<IActionResult> UpdateTheme(long id, UpdateThemeDto updateThemeDto)
        {
            await _themeService.UpdateTheme(id, updateThemeDto);

            return RedirectToAction("AllThemes", "Themes");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteTheme(long id)
        {
            await _themeService.DeleteTheme(id);

            return RedirectToAction("AllThemes", "Themes");
        }

    }
}
