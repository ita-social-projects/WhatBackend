using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Theme;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/theme")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly IThemeService _themeService;

        public ThemesController(IThemeService themeService)
        {
            _themeService = themeService;
        }

        [HttpPost]
        public async Task<ActionResult> PostTheme(CreateThemeModel themeModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var createdTheme = await _themeService.CreateThemeAsync(themeModel);
            if (themeModel == null) return StatusCode(500);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<ThemeModel>>> GetAllThemes()
        {
            try
            {
                var themes = await _themeService.GetAllThemesAsync();
                return Ok(themes);
            }
            catch { return StatusCode(500); }
        }
    }
}