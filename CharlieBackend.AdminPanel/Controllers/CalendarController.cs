using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _calendarService.GetCalendarDataAsync(new ScheduledEventFilterRequestDTO());

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetCalendar(ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            var data = await _calendarService.GetCalendarDataAsync(scheduledEventFilter);

            return View("Index", data);
        }
    }
}
