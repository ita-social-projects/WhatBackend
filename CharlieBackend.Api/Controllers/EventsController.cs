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
    /// Controller to manage scheduled events data
    /// </summary>
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;


        /// <summary>
        /// Events controller constructor
        /// </summary>
        /// <param name="eventsService"></param>
        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        /// <summary>
        /// Get event by id
        /// </summary>
        /// <response code = "200" > Successful getting of schedule</response>
        /// <response code="HTTP: 404">Error, given scheduled event not found</response>
        [SwaggerResponse(200, type: typeof(ScheduledEventDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledEventDTO>> GetEvent(long id)
        {
            return await _eventsService.GetAsync(id);
        }

        /// <summary>
        /// Updates a single event
        /// </summary>
        /// <response code="200">Successful update of schedule</response>
        /// <response code="HTTP: 400">Error, update data is wrong</response>
        /// <response code="HTTP: 404">Error, scheduled event does not exist</response>
        [SwaggerResponse(200, type: typeof(ScheduledEventDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ScheduledEventDTO>> UpdateEvent(long id, [FromBody] UpdateScheduledEventDto request)
        {
            return await _eventsService.UpdateAsync(id, request);
        }

        /// <summary>
        /// Deletes event by id
        /// </summary>
        /// <remarks>
        /// Removes one concrete scheduled event related to specified EventOccurrence
        /// Returns true if deleting was done, false in case scheduled event doesn't exist and error after the wrong request
        /// </remarks>
        /// <response code = "200" > Successful delete of schedule</response>
        /// /// <response code="HTTP: 404">Scheduled event does not exist</response>
        [Authorize(Roles = "Secretary, Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(long id)
        {
            var result = await _eventsService.DeleteAsync(id);

            return result.ToActionResult();
        }
    }
}
