using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Event;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage scheduled events data
    /// </summary>
    [Route("api/v{version:apiVersion}/events")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
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
        /// Add single event
        /// </summary>
        /// <response code="200">Successful add of schedule</response>
        /// <response code="HTTP: 400, API: 0">Can not create event due to wrong request data</response>
        /// <response code="HTTP: 404, API: 3">Can not create event due to missing request data</response>
        [SwaggerResponse(200, type: typeof(CreateSingleEventDTO))]
        [Authorize(Roles = "Secretary, Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateSingleEventDTO>> AddSingleEvent([FromBody] SingleEventRequestDto request)
        {
            var createEvent = await _eventsService.CreateSingleEvent(request);

            return createEvent.ToActionResult();
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

        /// <summary>
        /// Connects scheduled event to lesson by ids
        /// </summary>
        /// <param name="id">Id of scheduled event</param>
        /// <param name="lessonId">Lesson to be connected (Its enough to specify lessonId)</param>
        /// <response code = "200" > Successful event lesson connection </response>
        /// <response code="HTTP: 404, API: 3">Error, given scheduled event not found</response>
        [Authorize(Roles = "Secretary, Admin, Mentor")]
        [MapToApiVersion("2.0")]
        [HttpPatch("connect/{id:long}")]
        public async Task<ActionResult<EventOccurrenceDTO>> ConnectEventToLesson([FromRoute]long id, [FromQuery] long lessonId)
        {
            var foundSchedules = await _eventsService.ConnectScheduleToLessonById(id, lessonId);

            return foundSchedules.ToActionResult();
        }


    }
}
