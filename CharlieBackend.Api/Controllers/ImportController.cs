using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.DTO.File;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.FileModels;
using System.Collections.Generic;
using CharlieBackend.Business.Services;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage import from external sources
    /// </summary>
    [Route("api/imports")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly StudentImportService _studentImportService;
        private readonly GroupImportService _groupImportService;

        /// <summary>
        /// Import controller constructor
        /// </summary>
        public ImportController(StudentImportService studentImportService, GroupImportService groupImportService)
        {
            _studentImportService = studentImportService;
            _groupImportService = groupImportService;
        }

        /// <summary>
        /// Imports data from file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(List<StudentGroupFileModel>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [HttpPost]
        public async Task<ActionResult> ImportGroupDataFromFile(ImportFileDto file)
        {
            var listOfImportedGroups = await _groupImportService.ImportFileAsync(file);

            return listOfImportedGroups.ToActionResult();
        }

        /// <summary>
        /// Imports student data from file to database
        /// </summary>
        /// <response code="200">Successful import of data from file</response>
        /// <response code="HTTP: 400, API: 4">File validation error</response>
        [SwaggerResponse(200, type: typeof(List<StudentGroupFileModel>))]
        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [HttpPost]
        public async Task<ActionResult> ImportStudentDataFromFile(ImportFileDto file)
        {
            var listOfImportedStudents = await _studentImportService.ImportFileAsync(file);

            return listOfImportedStudents.ToActionResult();
        }
    }
}
