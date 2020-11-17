using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.DTO.File;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/imports")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IFileImportService _importService;

        public ImportController(IFileImportService importService)
        {
            _importService = importService;
        }

        [Authorize(Roles = "Mentor, Secretary, Admin")]
        [Route("student_groups")]
        [HttpPost]
        public async Task<ActionResult> ImportDataFromFile(ImportFileDto file)
        {
            var listOfImportedGroups = await _importService.ImportFileAsync(file);

            return listOfImportedGroups.ToActionResult();
        }
    }
}
