using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> PostTheme(ThemeModel themeModel)
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