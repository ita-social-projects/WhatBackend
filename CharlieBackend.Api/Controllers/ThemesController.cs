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
        #region
        private readonly IThemeService _themeService;
        #endregion

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
            catch 
            { 
                return StatusCode(500); 
            }
        }
    }
}