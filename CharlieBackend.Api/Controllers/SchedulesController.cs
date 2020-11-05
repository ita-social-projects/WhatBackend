using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Core;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        public readonly IScheduleService _scheduleService;
        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [Authorize(Roles = "4")]
        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> PostSchedule(CreateScheduleDto scheduleDTO)
        {
            Console.WriteLine("SSSSSS");
            var resSchedule = await _scheduleService
                .CreateScheduleAsync(scheduleDTO);

            return resSchedule.ToActionResult();
        }
    }
}
