using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.DTO.Visit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to make operations with homework
    /// </summary>
    [Route("api/v{version:apiVersion}/homeworks")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class HomeworksController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        /// <summary>
        /// Homework controllers constructor
        /// </summary>
        public HomeworksController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        /// <summary>
        /// Adds homework
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPost]
        public async Task<ActionResult> PostHomework([FromBody] HomeworkRequestDto request)
        {
            var results = await _homeworkService
                        .CreateHomeworkAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets homework by id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor, Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetHomeworkById(long id)
        {
            var results = await _homeworkService
                        .GetHomeworkByIdAsync(id);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets conditions of homeworks
        /// </summary>
        /// <param name="request">
        /// 1. Mention "groupId" or "courseId" or "themeId" to get homeworks of a specific group, course, theme.
        /// 2. You can mention optional parameters — "startDate", "finishtDate" to get homeworks depending on publishing date of homework
        /// </param>
        [SwaggerResponse(200, type: typeof(List<HomeworkDto>))]
        [Authorize(Roles = "Mentor, Admin, Secretary")]
        [HttpPost("getHomeworks")]
        public async Task<ActionResult> GetHomeworks([FromBody] GetHomeworkRequestDto request)
        {
            var results = await _homeworkService
                        .GetHomeworksAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Update homework
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkDto))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutHomework(long id, [FromBody] HomeworkRequestDto updateHomeworkDto)
        {
            var results = await _homeworkService
                        .UpdateHomeworkAsync(id, updateHomeworkDto);

            return results.ToActionResult();
        }

        /// <summary>
        /// Update student mark
        /// </summary>
        /// <response code="200">Successful updating of the mark</response>
        /// <response code="HTTP: 404">Student homework not found</response>
        [SwaggerResponse(200, type: typeof(VisitDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [MapToApiVersion("1.0")]
        [HttpPut("updatemark")]
        public async Task<ActionResult> UpdateMark([FromBody] UpdateMarkRequestDto request)
        {
            return (await _homeworkService.UpdateMarkAsync(request)).ToActionResult();
        }
    }
}
