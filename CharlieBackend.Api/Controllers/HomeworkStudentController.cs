using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to make operations with homework from student
    /// </summary>
    [Route("api/v{version:apiVersion}/homeworkstudent")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]

    public class HomeworkStudentController : ControllerBase
    {

        private readonly IHomeworkStudentService _homeworkStudentService;

        /// <summary>
        /// HomeworkStudent controllers constructor
        /// </summary>
        public HomeworkStudentController(IHomeworkStudentService homeworkStudentService)
        {
            _homeworkStudentService = homeworkStudentService;
        }

        /// <summary>
        /// Add homework solution from student
        /// </summary>
        /// <param name="request">
        /// 1.HomeworkId -  it is id homework which one student done 
        /// 2.HomeworkText - here Student can write his Homework or something else
        /// 3.Attacment(LIst) -   here must be Id of Attachment where student uploaded materials for his Homework</param>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<ActionResult> PostHomework([FromBody] HomeworkStudentRequestDto request)
        {
            var results = await _homeworkStudentService.CreateHomeworkFromStudentAsync(request);

            return results.ToActionResult();
        }

        /// <summary>
        /// Get homework solutions by student id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IList<HomeworkStudentDto>> GetHomeworkForStudentByStudentId()
        {
            var results = await _homeworkStudentService.GetHomeworkStudentForStudent();

            return results;
        }

        /// <summary>
        /// Get the completed homework solutions of a student in a group
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Student")]
        [HttpGet("done")]
        public async Task<ActionResult> GetComplitedHomeworkStudentByFilter([FromQuery] HomeworkStudentFilter homeworkForStudent)
        {
            var results = await _homeworkStudentService.GetComplitedHomeworkStudentByFilter(homeworkForStudent);

            return results.ToActionResult();
        }

        /// <summary>
        /// Get student homework solution for Mentor by homework task id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Mentor")]
        [HttpGet("{homeworkId}")]
        public async Task<Result<IList<HomeworkStudentDto>>> GetHomeworkForMentorByHomeworkId(long homeworkId)
        {
            var results = await _homeworkStudentService.GetHomeworkStudentForMentor(homeworkId);

            return results;
        }

        /// <summary>
        ///Get student homework solution history for Mentor by student homework solution id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Mentor, Admin, Secretary")]
        [MapToApiVersion("2.0")]
        [HttpGet("history/{homeworkStudentId}")]
        public async Task<IList<HomeworkStudentDto>> GetHomeworkStudentsHistoryByHomeworkStudentId(long homeworkStudentId)
        {
            var results = await _homeworkStudentService.GetHomeworkStudentHistoryByHomeworkStudentId(homeworkStudentId);

            return results;
        }

        /// <summary>
        /// Update student homework solution
        /// </summary>
        /// <param name="id">
        ///  Id - it is Student homework id </param>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Student")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutHomework(long id, [FromBody] HomeworkStudentRequestDto updateHomeworkDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var results = await _homeworkStudentService.UpdateHomeworkFromStudentAsync(updateHomeworkDto, id);

            return results.ToActionResult();
        }

        /// <summary>
        /// Update student mark for homework solution
        /// </summary>
        /// <response code="200">Successful updating of the mark</response>
        /// <response code="HTTP: 404">Student homework not found</response>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [MapToApiVersion("2.0")]
        [HttpPut("updatemark")]
        public async Task<ActionResult> UpdateMark([FromBody] UpdateMarkRequestDto request)
        {
            return (await _homeworkStudentService.UpdateMarkAsync(request)).ToActionResult();
        }
    }
}
