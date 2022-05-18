using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Panel.Models.Calendar;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Student")]
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
        public async Task<IActionResult> GetCalendar(ScheduledEventFilterRequestDTO scheduledEventFilter, CalendarDisplayType displayType)
        {
            var calendarData = await _calendarService.GetCalendarDataAsync(scheduledEventFilter);

            calendarData.DisplayType = displayType;

            return View("Index", calendarData);
        }
    }
}
