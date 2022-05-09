using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor")]
    [Route("[controller]/[action]")]
    public class EventColorController : Controller
    {
        private readonly IEventColorService _eventColorService;

        public EventColorController(IEventColorService eventColorService)
        {
            _eventColorService = eventColorService;
        }
        public async Task<IActionResult> AllEventColors()
        {
            var colors = await _eventColorService.GetAllEventColorsAsync();

            return View(colors);
        }
    }
}
