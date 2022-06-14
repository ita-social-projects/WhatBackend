using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Mark;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Marks controller
    /// </summary>
    [Route("api/v{version:apiVersion}/marks")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class MarksController : ControllerBase
    {
        private readonly IMarkService _markService;

        /// <summary>
        /// Marks controller constructor
        /// </summary>
        public MarksController(IMarkService markService)
        {
            _markService = markService;
        }

        /// <summary>
        /// Gets mark by id
        /// </summary>
        /// <response code="200">Successful return of mark</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find mark</response>
        [SwaggerResponse(200, type: typeof(MarkDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MarkDto>> GetMarkById(long id)
        {
            var foundMark = await _markService.GetMarkByIdAsync(id);

            return foundMark.ToActionResult();
        }
    }
}
