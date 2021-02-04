﻿using System;
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
        /// Get event occurrence by id
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
        /// Updates range of events
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, update data is missing</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(EventOccurenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("events/updateRange")]
        public async Task<ActionResult<EventOccurenceDTO>> UpdateEventRange([FromBody]EventUpdateRangeDTO request)
        {
            var foundSchedules = await _scheduleService.UpdateEventsRange(request.Filter, request.Request);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Updates a single event
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, update data is missing</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(EventOccurrenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("events/{scheduledEventID}")]
        public async Task<ActionResult<EventOccurenceDTO>> UpdateEventById(long scheduledEventID, [FromBody]UpdateScheduledEventDto updateScheduleDto)
        {
            var foundSchedules = await _scheduleService.UpdateScheduledEventByID(scheduledEventID, updateScheduleDto);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Updates a single event
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, update data is missing</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(EventOccurenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("scheduledEvents/{eventOccurrenceID}")]
        public async Task<ActionResult<EventOccurenceDTO>> UpdateEventOccurrenceById(long eventOccurrenceID, [FromBody]CreateScheduleDto updateOccurrenceRequest)
        {
            var foundSchedules = await _scheduleService.UpdateEventOccurrenceById(eventOccurrenceID, updateOccurrenceRequest);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Deletes exact schedule
        /// </summary>
        /// <response code = "200" > Successful delete of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, given schedule not found</response>
        [Authorize(Roles = "Secretary, Admin")]
        [HttpDelete("{eventOccurrenceID}")]
        public async Task<ActionResult<EventOccurenceDTO>> DeleteSchedule(long scheduleId, DateTime? startDate, DateTime? finishDate)
        {
            var foundSchedules = await _scheduleService.DeleteScheduleByIdAsync(scheduleId, startDate, finishDate);

            return foundSchedules.ToActionResult();
        }
    }
}
