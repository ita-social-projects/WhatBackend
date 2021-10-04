using CharlieBackend.Business.Services.FileServices.ImportFileServices;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
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
        IServiseImport _servise;

        /// <summary>
        /// Import controller constructor
        /// </summary>
        public ImportController(IServiseImport servise)
        {
            _servise = servise;
        }

        /// <summary>
        /// Imports group with students data from .xlsx or csv file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(IEnumerable<GroupWithStudentsDto>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("group/{courseId}")]
        [HttpPost]
        public async Task<ActionResult> ImportGroupWithStudentsDataFromFile(
                IFormFile file, long courseId, string groupName = null,
                DateTime startDate = default, DateTime finishDate = default)
        {
            var result = await _servise.ImportGroupAsync(file,
                    new CreateStudentGroupDto()
                    {
                        CourseId = courseId,
                        Name = groupName,
                        StartDate = startDate,
                        FinishDate = finishDate,
                    });

            return result.ToActionResult();
        }

        /// <summary>
        /// Imports student data from .xlsx or csv file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(IEnumerable<ThemeDto>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("themes")]
        [HttpPost]
        public async Task<ActionResult> ImportThemeDataFromFile(IFormFile file)
        {
            var result = await _servise.ImportThemesAsync(file);

            return result.ToActionResult();
        }
    }
}
