﻿using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to namage themes
    /// </summary>
    [Route("api/v{version:apiVersion}/themes")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly IThemeService _themeService;

        /// <summary>
        /// Themes controllers constructor
        /// </summary>
        /// <param name="themeService"></param>
        public ThemesController(IThemeService themeService)
        {
            _themeService = themeService;
        }

        /// <summary>
        /// Get all themes
        /// </summary>
        /// <response code="200">Successful return of themes list</response>
        [SwaggerResponse(200, type: typeof(IList<ThemeDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        public async Task<ActionResult<IList<ThemeDto>>> GetAllThemes()
        {
            var themes = await _themeService.GetAllThemesAsync();

            return themes.ToActionResult();
        }

        /// <summary>
        /// Addition of new theme
        /// </summary>
        /// <response code="200">Successful create of theme</response>
        [SwaggerResponse(200, type: typeof(ThemeDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost]
        public async Task<ActionResult<IList<ThemeDto>>> PostThemes([FromBody] CreateThemeDto addThemeModel)
        {
            var themeResult = await _themeService.CreateThemeAsync(addThemeModel);

            return themeResult.ToActionResult();
        }

        /// <summary>
        /// Update of existing theme
        /// </summary>
        /// <response code="200">Successful update of theme</response>
        /// <response code="HTTP: 404, API: 3">Error, can not handle update data</response>
        [SwaggerResponse(200, type: typeof(ThemeDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPut("{themeId}")]
        public async Task<ActionResult<IList<ThemeDto>>> UpdateTheme(long themeId, [FromBody] UpdateThemeDto UpdateThemeModel)
        {
            var themeResult = await _themeService.UpdateThemeAsync(themeId, UpdateThemeModel);

            return themeResult.ToActionResult();
        }

        /// <summary>
        /// Deletion of theme
        /// </summary>
        /// <response code="200">Successful deletion of theme</response>
        /// <response code="HTTP: 404, API: 3">Error, can not delete theme</response>
        [SwaggerResponse(200, type: typeof(ThemeDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpDelete("{themeId}")]
        public async Task<ActionResult<IList<ThemeDto>>> DeleteTheme(long themeId)
        {
            var themeResult = await _themeService.DeleteThemeAsync(themeId);

            return themeResult.ToActionResult();
        }
    }
}
