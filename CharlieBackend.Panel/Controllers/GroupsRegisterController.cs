using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
    public class GroupsRegisterController : Controller
    {
        private readonly IGroupsRegisterService _groupsRegisterService;

        public GroupsRegisterController(IGroupsRegisterService groupsRegistersService)
        {
            _groupsRegisterService = groupsRegistersService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _groupsRegisterService.GetCalendarDataAsync(new RegistersFilterRequestDTO());

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetRegister(ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            var data = await _groupsRegistersService.GetCalendarDataAsync(scheduledEventFilter);

            return View("Index", data);
        }
    }
}
