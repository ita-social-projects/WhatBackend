using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class EventOccurrenceController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public EventOccurrenceController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEventOccurrences()
        {
            var allEventOccurences = await _scheduleService.GetAllEventOccurrences();

            return View(allEventOccurences);
        }
    }
}
