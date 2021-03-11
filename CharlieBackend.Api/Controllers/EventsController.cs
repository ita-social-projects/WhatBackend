using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage schedules data
    /// </summary>
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;


        /// <summary>
        /// Events controller constructor
        /// </summary>
        /// <param name="scheduleService"></param>
        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        /// <summary>
        /// Get event by id
        /// </summary>
        /// <response code = "200" > Successful getting of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, given schedule event not found</response>
        [SwaggerResponse(200, type: typeof(ScheduledEventDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{scheduledEventID}")]
        public async Task<ActionResult<ScheduledEventDTO>> GetConcreteScheduleByID(long scheduledEventID)
        {
            var foundScheduleEvent = await _eventsService.GetConcreteScheduleByIdAsync(scheduledEventID);

            return foundScheduleEvent.ToActionResult();
        }

        /// <summary>
        /// Updates a single event
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 404, API: 3">Error, update data is missing</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(EventOccurrenceDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("{scheduledEventID}")]
        public async Task<ActionResult<ScheduledEventDTO>> UpdateEventById(long scheduledEventID, [FromBody] UpdateScheduledEventDto request)
        {
            var foundSchedules = await _eventsService.UpdateScheduledEventByID(scheduledEventID, request);

            return foundSchedules.ToActionResult();
        }

        /// <summary>
        /// Deletes event by id
        /// </summary>
        /// <remarks>
        /// Removes one concrete scheduled event related to specified EventOccurrence
        /// Returns true if deleting was done, false in case scheduled event doesn't exist and error after the wrong request
        /// </remarks>
        /// <response code = "200" > Successful delete of schedule</response>
        /// /// <response code="HTTP: 400, API: 0">Scheduled event does not exist</response>
        /// <response code="HTTP: 409, API: 5">Can not delete scheduled event due to wrong request data</response>
        [Authorize(Roles = "Secretary, Admin")]
        [HttpDelete("{scheduledEventID}")]
        public async Task<ActionResult> DeleteConcreteSchedule(long scheduledEventID)
        {
            var result = await _eventsService.DeleteConcreteScheduleByIdAsync(scheduledEventID);

            return result.ToActionResult();
        }
    }
}
