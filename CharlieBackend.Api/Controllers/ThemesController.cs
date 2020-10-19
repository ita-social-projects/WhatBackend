using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Core.Models.Theme;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

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

        [Authorize(Roles = "2, 4")]
        [HttpGet]
        public async Task<ActionResult<IList<ThemeModel>>> GetAllThemes()
        {

            var themes = await _themeService.GetAllThemesAsync();

            return Ok(themes.Select(theme => theme.Name));
        }

        [Authorize(Roles = "2, 4")]
        [HttpPost]
        public async Task<ActionResult<IList<ThemeModel>>> PostThemes([FromBody] ThemeModel addThemeModel)
        {

            await _themeService.CreateThemeAsync(addThemeModel);

            return Ok(addThemeModel);

        }

    }
}