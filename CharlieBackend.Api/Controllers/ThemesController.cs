using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<List<ThemeModel>>> GetAllThemes()
        {
            try
            {
                var themes = await _themeService.GetAllThemesAsync();
                return Ok(themes.Select(theme => theme.Name));
            }
            catch { return StatusCode(500); }
        }
    }
}