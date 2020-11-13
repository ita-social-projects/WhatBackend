using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Theme;


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

            return Ok(themes);
        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost]
        public async Task<ActionResult<IList<ThemeDto>>> PostThemes(CreateThemeDto addThemeModel)
        {

            var themeResult = await _themeService.CreateThemeAsync(addThemeModel);

            return Ok(themeResult);

        }

    }
}