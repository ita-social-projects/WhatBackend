using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/v{version:apiVersion}/colors")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class EventColorController : ControllerBase
    {

        private readonly IEventColorService _eventColorService;

        public EventColorController(IEventColorService eventColorService)
        {
            _eventColorService = eventColorService;
        }
        /// <summary>
        /// Get all event colors
        /// </summary>
        /// <response code="200">Successful return of colors list</response>
        [SwaggerResponse(200, type: typeof(IList<EventColorDTO>))]
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        public async Task<ActionResult<IList<EventColorDTO>>> GetAllEventColors()
        {
            var colors = await _eventColorService.GetAllEventColorsAsync();

            return colors.ToActionResult();
        }

        /// <summary>
        /// Get event color by Id
        /// </summary>
        /// <response code="200">Successful return of color by ID</response>
        [HttpGet("id")]
        public async Task<ActionResult<EventColorDTO>> GetEventColorById(long id)
        {
            var color = await _eventColorService.GetEventColorByIdAsync(id);

            return color.ToActionResult();
        }
    }
}
