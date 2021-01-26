using CharlieBackend.Core.DTO.HomeworkStudent;
using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Core;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to make operations with homework from student
    /// </summary>
    [Route("api/homeworkstudent")]
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
        /// Adds homework from student
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Student")] //?? admin 
        [HttpPost]
        public async Task<ActionResult> PostHomework([FromBody] HomeworkStudentRequestDto request)
        {
            var context = HttpContext.User;

            var results = await _homeworkStudentService.CreateHomeworkFromStudentAsync(request, context);

            return results.ToActionResult();
        }

        /// <summary>
        /// Gets student homework by student id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IList<HomeworkStudentDto>> GetHomeworkForStudentByStudentId()
        {
            var userContext = HttpContext.User;
            var results = await _homeworkStudentService.GetHomeworkStudentForStudent(userContext);

            return results;
        }

        /// <summary>
        /// Gets student homework for Mentor by homework id
        /// </summary>
        [SwaggerResponse(200, type: typeof(HomeworkStudentDto))]
        [Authorize(Roles = "mentor")]
        [HttpGet("{homeworkId}")]
        public async Task<IList<HomeworkStudentDto>> GetHomeworkForMentorByHomeworkId(long homeworkId)
        {
            var userContext = HttpContext.User;
            var results = await _homeworkStudentService.GetHomeworkStudentForMentorByHomeworkId(homeworkId, userContext);

            return results;
        }
    }
}
