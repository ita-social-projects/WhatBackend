using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/themes")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly IThemeService _themeService;

        public ThemesController(IThemeService themeService)
        {
            _themeService = themeService;
        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet]
        public async Task<ActionResult<IList<ThemeDto>>> GetAllThemes()
        {

            var themes = await _themeService.GetAllThemesAsync();

            return themes.ToActionResult();
        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost]
        public async Task<ActionResult<IList<ThemeDto>>> PostThemes(CreateThemeDto addThemeModel)
        {

            var themeResult = await _themeService.CreateThemeAsync(addThemeModel);

            return themeResult.ToActionResult();

        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPut("{themeId}")]
        public async Task<ActionResult<IList<ThemeDto>>> UpdateTheme(long themeId, UpdateThemeDto UpdateThemeModel)
        {

            var themeResult = await _themeService.UpdateThemeAsync(themeId, UpdateThemeModel);

            return themeResult.ToActionResult();

        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpDelete("{themeId}")]
        public async Task<ActionResult<IList<ThemeDto>>> DeleteTheme(long themeId)
        {

            var themeResult = await _themeService.DeleteThemeAsync(themeId);

            return themeResult.ToActionResult();

        }

    }
}