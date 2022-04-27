using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Panel.Models.Classbook;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
    public class ClassbookController : Controller
    {
        private readonly IClassbookService _classbookService;

        public ClassbookController(IClassbookService classbookService)
        {
            _classbookService = classbookService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var param = new StudentsRequestDto<ClassbookResultType>();
            ClassbookViewModel data = await _classbookService.GetClassbookAsync(param);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetClassbookAsync(StudentsRequestDto<ClassbookResultType> scheduledEventFilter)
        {
            ClassbookViewModel data = await _classbookService.GetClassbookAsync(scheduledEventFilter);

            return View("Index", data);
        }
    }
}
