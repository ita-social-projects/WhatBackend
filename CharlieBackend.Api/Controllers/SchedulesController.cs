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

        [Authorize(Roles = "Secretary, Admin")]
        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> PostSchedule(CreateScheduleDto scheduleDTO)
        {
            var resSchedule = await _scheduleService
                .CreateScheduleAsync(scheduleDTO);
                
            return resSchedule.ToActionResult();
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<ScheduleDto>>> GetAllSchedules()
        {
            return Ok(await _scheduleService.GetAllSchedulesAsync());
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{scheduleId}")]
        public async Task<ActionResult<ScheduleDto>> GetScheduleById(long scheduleId)
        {
            var foundSchedule = await _scheduleService.GetScheduleByIdAsync(scheduleId);

            return foundSchedule.ToActionResult();
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{studentGroupId}/groupSchedule")]
        public async Task<ActionResult<List<ScheduleDto>>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
            var foundSchedules = await _scheduleService.GetSchedulesByStudentGroupIdAsync(studentGroupId);

            return foundSchedules == null ? 
                Result<ScheduleDto>.Error(ErrorCode.NotFound, "studentGroupId is not valid").ToActionResult() :
                Ok(foundSchedules);
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("{scheduleId}")]
        public async Task<ActionResult<ScheduleDto>> PutSchedule(long scheduleId, UpdateScheduleDto updateScheduleDto)
        {
            var foundSchedules = await _scheduleService.UpdateStudentGroupAsync(scheduleId, updateScheduleDto);

            return foundSchedules.ToActionResult();
        }

        [Authorize(Roles = "Secretary, Admin")]
        [HttpDelete("{scheduleId}")]
        public async Task<ActionResult<ScheduleDto>> DeleteSchedule(long scheduleId)
        {
            var foundSchedules = await _scheduleService.DeleteScheduleByIdAsync(scheduleId);

            return foundSchedules.ToActionResult();
        }
    }
}
