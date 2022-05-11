using CharlieBackend.Panel.Models.Export;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Panel.Extensions;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
    [Route("[controller]")]
    public class ExportController : Controller
    {
        private readonly IExportService _exportService;

        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpPost]
        [Route("studentsClassbooks/{format}")]
        public async Task<IActionResult> ExportStudentsClassbooks(ExportFileFormat format, ExportByCourseAndStudentGroupModel exportModel)
        {
            var fileBytes = await _exportService.ExportStudentsClassbook(format, exportModel);

            return this.ExportFile(fileBytes, format);
        }
    }
}