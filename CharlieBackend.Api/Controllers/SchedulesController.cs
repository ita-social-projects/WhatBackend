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
using CharlieBackend.Core.Models.ResultModel;

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

        [Authorize(Roles = "3,4")]
        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> PostSchedule(CreateScheduleDto scheduleDTO)
        {
            var resSchedule = await _scheduleService
                .CreateScheduleAsync(scheduleDTO);
            return resSchedule.ToActionResult();
        }

        [Authorize(Roles = "3,4")]
        [HttpGet]
        public async Task<ActionResult<List<ScheduleDto>>> GetAllSchedules()
        {
            return Ok(await _scheduleService.GetAllSchedulesAsync());
        }

        [Authorize(Roles = "3,4")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetScheduleById(long id)
        {
            var foundSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            return foundSchedule.ToActionResult();
        }

        [Authorize(Roles = "3,4")]
        [HttpGet("{studentGroupId}")]
        public async Task<ActionResult<List<ScheduleDto>>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
            var foundSchedules = await _scheduleService.GetSchedulesByStudentGroupIdAsync(studentGroupId);
            return foundSchedules == null ? 
                Result<ScheduleDto>.Error(ErrorCode.NotFound, "Schedule id is not valid").ToActionResult() :
                Ok(foundSchedules);
        }
    }
}
