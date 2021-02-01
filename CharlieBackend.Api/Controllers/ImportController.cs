﻿using CharlieBackend.Business.Services.FileServices;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Student;
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
        private readonly IXLSFileService _xlsFileService;

        /// <summary>
        /// Import controller constructor
        /// </summary>
        public ImportController(IXLSFileService xlsFileService)
        {
            _xlsFileService = xlsFileService;
        }

        /// <summary>
        /// Imports group data from .xlsx file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(IEnumerable<ImportStudentGroupDto>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("groups/{coursId}")]
        [HttpPost]
        public async Task<ActionResult> ImportGroupDataFromFile(long coursId, IFormFile file)
        {
            var groups = await _xlsFileService.ImportGroupsAsync(coursId, file);

            return groups.ToActionResult();
        }

        /// <summary>
        /// Imports student data from .xlsx file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(IEnumerable<StudentDto>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("students/{groupId}")]
        [HttpPost]
        public async Task<ActionResult> ImportStudentDataFromFile(long groupId, IFormFile file)
        {
            var students = await _xlsFileService.ImportStudentsAsync(groupId, file);

            return students.ToActionResult();
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
            var themes = await _xlsFileService.ImportThemesAsync(file);

            return themes.ToActionResult();
        }
    }
}
