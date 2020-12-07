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
using Swashbuckle.AspNetCore.Annotations;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage schedules data
    /// </summary>
    [Route("api/schedules")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly IScheduleService _scheduleService;

        /// <summary>
        /// Schedules cocontroller constructor
        /// </summary>
        /// <param name="scheduleService"></param>
        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// Add new schedule
        /// </summary>
        /// <response code="200">Successful add of schedule</response>
        /// <response code="HTTP: 400, API: 0">Can not create schedule due to wrong request data</response>
        /// <response code="HTTP: 404, API: 3">Can not create schedule due to missing request data</response>
        [SwaggerResponse(200, type: typeof(ScheduleDto))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> PostSchedule([FromBody]CreateScheduleDto scheduleDTO)
        {
            var resSchedule = await _scheduleService
                .CreateScheduleAsync(scheduleDTO);
                
            return resSchedule.ToActionResult();
        }

        /// <summary>
        /// Gets all schedules
        /// </summary>
        /// <response code="200">Successful return of schedules list</response>
        [SwaggerResponse(200, type: typeof(List<ScheduleDto>))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<ScheduleDto>>> GetAllSchedules()
        {
            var resSchedule = await _scheduleService.GetAllSchedulesAsync();
            return resSchedule.ToActionResult();
        }

        /// <summary>
        /// Returns schedules of exact student group
        /// </summary>
        /// <response code="200">Successful return of schedules list</response>
        /// <response code="HTTP: 404, API: 3">Error, student group not found</response>
        [SwaggerResponse(200, type: typeof(IList<ScheduleDto>))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{studentGroupId}/groupSchedule")]
        public async Task<ActionResult<List<ScheduleDto>>> GetSchedulesByStudentGroupIdAsync(long studentGroupId)
        {
            var foundSchedules = await _scheduleService.GetSchedulesByStudentGroupIdAsync(studentGroupId);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Updates shedule
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, update data is missing</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(ScheduleDto))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("{scheduleId}")]
        public async Task<ActionResult<ScheduleDto>> PutSchedule(long scheduleId, [FromBody]UpdateScheduleDto updateScheduleDto)
        {
            var foundSchedules = await _scheduleService.UpdateStudentGroupAsync(scheduleId, updateScheduleDto);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Deletes exact schedule
        /// </summary>
        /// <response code = "200" > Successful delete of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, given schedule not found</response>
        [Authorize(Roles = "Secretary, Admin")]
        [HttpDelete("{scheduleId}")]
        public async Task<ActionResult<ScheduleDto>> DeleteSchedule(long scheduleId)
        {
            var foundSchedules = await _scheduleService.DeleteScheduleByIdAsync(scheduleId);

            return foundSchedules.ToActionResult();
        }
    }
}
