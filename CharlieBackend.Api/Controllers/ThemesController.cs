﻿using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.DTO.Homework;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to namage themes
    /// </summary>
    [Route("api/themes")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly IThemeService _themeService;
        private readonly IHomeworkService _homeworkService;

        /// <summary>
        /// Themes controllers constructor
        /// </summary>
        /// <param name="themeService"></param>
        /// <param name="homeworkService"></param>
        public ThemesController(IThemeService themeService, IHomeworkService homeworkService)
        {
            _themeService = themeService;
            _homeworkService = homeworkService;
        }

        /// <summary>
        /// Get all themes
        /// </summary>
        /// <response code="200">Successful return of themes list</response>
        [SwaggerResponse(200, type: typeof(IList<ThemeDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
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
        public async Task<ActionResult<IList<ThemeDto>>> PostThemes([FromBody]CreateThemeDto addThemeModel)
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
        public async Task<ActionResult<IList<ThemeDto>>> UpdateTheme(long themeId, [FromBody]UpdateThemeDto UpdateThemeModel)
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

        /// <summary>
        /// Gets homeworks by theme id
        /// </summary>
        [SwaggerResponse(200, type: typeof(List<HomeworkDto>))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet("{id}/hometasks")]
        public async Task<ActionResult> GetHomeworksById(long id)
        {
            var results = await _homeworkService
                        .GetHomeworksByThemeId(id);

            return results.ToActionResult();
        }
    }
}