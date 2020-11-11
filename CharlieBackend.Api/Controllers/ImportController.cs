
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core.DTO.File;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/imports")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }


        [Authorize(Roles = "Mentor, Secretary, 4")]
        [HttpPost]
        public async Task<ActionResult> ImportDataFromFile(ImportFileDto file)
        {

            var listOfImportedGroups = await _importService.ImportFileAsync(file);
            return listOfImportedGroups.ToActionResult();

        }
    }
}
