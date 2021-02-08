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


        private readonly IScheduleService _scheduleService;


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
        /// <remarks>
        /// Creates new EventOccurance instance and related ScheduledEvents
        /// Information on input format could be found here: https://docs.microsoft.com/en-us/graph/outlook-schedule-recurring-events
        /// </remarks>
        /// <response code="200">Successful add of schedule</response>
        /// <response code="HTTP: 400, API: 0">Can not create schedule due to wrong request data</response>
        /// <response code="HTTP: 404, API: 3">Can not create schedule due to missing request data</response>
        [SwaggerResponse(200, type: typeof(EventOccurrenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPost]
        public async Task<ActionResult<EventOccurrenceDTO>> PostSchedule([FromBody]CreateScheduleDto scheduleDTO)
        {
            var resSchedule = await _scheduleService
                .CreateScheduleAsync(scheduleDTO);
                
            return resSchedule.ToActionResult();
        }

        /// <summary>
        /// Get event occurance by id
        /// </summary>
        /// <response code="200">Successful add of schedule</response>        
        /// <response code="HTTP: 404, API: 3">No such event occurence</response>
        [SwaggerResponse(200, type: typeof(EventOccurrenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EventOccurrenceDTO>> GetEventOccuranceByID(long id)
        {
            var resSchedule = await _scheduleService.GetEventOccurrenceByIdAsync(id);

            return resSchedule.ToActionResult();
        }

        /// <summary>
        /// Gets all schedules
        /// </summary>
        /// <response code="200">Successful return of schedules list</response>
        [SwaggerResponse(200, type: typeof(List<EventOccurrenceDTO>))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<EventOccurrenceDTO>>> GetAllSchedules()
        {
            var resSchedule = await _scheduleService.GetAllSchedulesAsync();
            return resSchedule.ToActionResult();
        }

        /// <summary>
        /// Returns the list of events depending on the filtering rules set
        /// </summary>
        /// <response code="200">Successful return event list</response>
        [SwaggerResponse(200, type: typeof(IList<ScheduledEventDTO>))]
        [Authorize(Roles = "Secretary, Admin, Mentor, Student")]
        [HttpPost("events")]
        public async Task<ActionResult<List<ScheduledEventDTO>>> GetEventsFiltered(ScheduledEventFilterRequestDTO request)
        {
            var foundSchedules = await _scheduleService.GetEventsFiltered(request);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Updates shedule
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, update data is missing</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(EventOccurrenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("{scheduleId}")]
        public async Task<ActionResult<EventOccurrenceDTO>> PutSchedule(long scheduleId, [FromBody]UpdateScheduleDto updateScheduleDto)
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
        public async Task<ActionResult<EventOccurrenceDTO>> DeleteSchedule(long scheduleId)
        {
            var foundSchedules = await _scheduleService.DeleteScheduleByIdAsync(scheduleId);

            return foundSchedules.ToActionResult();
        }
    }
}
