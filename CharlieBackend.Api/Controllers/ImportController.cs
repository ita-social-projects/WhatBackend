﻿using CharlieBackend.Business.Services.FileServices;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage import from external sources
    /// </summary>
    [Route("api/imports")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportTemplatesService _importTemplatesService;
        private readonly IThemeXlsFileImporter _themeXlsFileImporter;
        private readonly IStudentsGroupXlsxFileImporter _studentsGroupXlsxFileImporter;

        /// <summary>
        /// Import controller constructor
        /// </summary>
        public ImportController(IImportTemplatesService importTemplatesService,
                                IThemeXlsFileImporter themeXlsFileImporter,
                                IStudentsGroupXlsxFileImporter studentsGroupXlsxFileImporter)
        {
            _importTemplatesService = importTemplatesService;
            _themeXlsFileImporter = themeXlsFileImporter;
            _studentsGroupXlsxFileImporter = studentsGroupXlsxFileImporter;
        }

        /// <summary>
        /// Imports group with students data from .xlsx file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(IEnumerable<GroupWithStudentsDto>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("group/{courseId}")]
        [HttpPost]
        public async Task<ActionResult> ImportGroupWithStudentsDataFromFile(long courseId, IFormFile file)
        {
            var groups = await _studentsGroupXlsxFileImporter.ImportGroupAsync(courseId, file);

            return groups.ToActionResult();
        }

        /// <summary>
        /// Imports student data from .xlsx file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(IEnumerable<ThemeDto>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("themes")]
        [HttpPost]
        public async Task<ActionResult> ImportThemeDataFromFile(IFormFile file)
        {
            var themes = await _themeXlsFileImporter.ImportThemesAsync(file);

            return themes.ToActionResult();
        }

        /// <summary>
        /// Returns template of group
        /// </summary>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("group")]
        [HttpGet]
        public async Task<ActionResult> GetTemplateForGroupImport()
        {
            var fileName = _importTemplatesService.GetGroupTemplateFileName();

            return File(await _importTemplatesService.GetByteArrayAsync(fileName),
                        _importTemplatesService.GetContentType(),
                        fileName);
        }

        /// <summary>
        /// Returns template of group
        /// </summary>
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("themes")]
        [HttpGet]
        public async Task<ActionResult> GetTemplateForThemeImport()
        {
            var fileName = _importTemplatesService.GetThemeTemplateFileName();

            return File(await _importTemplatesService.GetByteArrayAsync(fileName),
                        _importTemplatesService.GetContentType(),
                        fileName);
        }
    }
}
