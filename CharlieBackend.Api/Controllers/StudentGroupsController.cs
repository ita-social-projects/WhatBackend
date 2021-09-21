using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Homework;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Business.Services.Interfaces;
using System;
using CharlieBackend.Core.DTO.Lesson;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Student groups controller
    /// </summary>
    [Route("api/student_groups")]
    [ApiController]
    public class StudentGroupsController : ControllerBase
    {

        private readonly IStudentGroupService _studentGroupService;
        private readonly IHomeworkService _homeworkService;
        private readonly ILessonService _lessonService;

        /// <summary>
        /// Student Groups controllers constructor
        /// </summary>
        public StudentGroupsController(IStudentGroupService studentGroupService, IHomeworkService homeworkService, 
            ILessonService lessonService)
        {
            _studentGroupService = studentGroupService;
            _homeworkService = homeworkService;
            _lessonService = lessonService;
        }

        /// <summary>
        /// Deletes the student group
        /// </summary>
        ///<response code="200">Successful deletion of student group</response>
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<StudentGroupDto>>> DeleteStudentGroup(long id)
        {
            if (await _studentGroupService.DeleteStudentGroupAsync(id))
            {
                return Ok("Student group is deleted.");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns list of lessons for student group
        /// </summary>
        /// <response code="200">Successful return of lessons list of given student group</response>
        [SwaggerResponse(200, type: typeof(IList<StudentLessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<List<StudentLessonDto>>> GetAllLessonsForStudentGroup(long id)
        {
            var studentGroup = await _studentGroupService.GetStudentGroupByIdAsync(id);

            if (studentGroup.Data == null)
            {
                return NotFound();
            }
            else
            {
                var lessonsOfStudentGroup = _lessonService.GetAllLessonsForStudentGroup(id);

                return lessonsOfStudentGroup.Result.ToActionResult();
            }
        }

        /// <summary>
        /// Adding of new student group
        /// </summary>
        /// <response code="200">Successful group addition</response>
        /// <response code="HTTP: 422, API: 4">Error, given group data already exist</response>
        /// <response code="HTTP: 400, API: 0">Error, given group data unprocessable</response>
        [SwaggerResponse(200, type: typeof(StudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPost]
        public async Task<ActionResult<StudentGroupDto>> PostStudentGroup([FromBody]CreateStudentGroupDto studentGroup)
        {
            var resStudentGroup = await _studentGroupService.CreateStudentGroupAsync(studentGroup);
          
            return resStudentGroup.ToActionResult();
        }

        /// <summary>
        /// Updates given student group
        /// </summary>
        /// <response code="200">Successful update of student group</response>
        /// <response code="HTTP: 422, API: 4">Error, given group name already exist</response>
        /// <response code="HTTP: 400, API: 0">Error, given group data is wrong</response>
        [SwaggerResponse(200, type: typeof(UpdateStudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentGroupDto>> PutStudentGroup(long id, [FromBody]UpdateStudentGroupDto studentGroupDto)
        {
            var updatedStudentGroup = await _studentGroupService
                    .UpdateStudentGroupAsync(id, studentGroupDto);

            return updatedStudentGroup.ToActionResult();
        }

        /// <summary>
        /// Gets all student groups
        /// </summary>
        /// <remarks>
        /// Returns all groups of students in the active course for the specified period (if no dates are specified, returns all groups)
        /// </remarks>
        /// <response code="200">Successful return of students of student group</response>
        [SwaggerResponse(200, type: typeof(IList<StudentGroupDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<StudentGroupDto>>> GetAllStudentGroups(DateTime? startDate, DateTime? finishDate)
        {
            var groups = await _studentGroupService.GetAllStudentGroupsAsync(startDate, finishDate);

            return groups.ToActionResult();
        }

        /// <summary>
        /// Gets student group
        /// </summary>
        /// <response code="200">Successful return of student group</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find student group</response>
        [SwaggerResponse(200, type: typeof(StudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentGroupDto>> GetStudentGroupById(long id)
        {
            var foundStudentGroup = await _studentGroupService
                    .GetStudentGroupByIdAsync(id);

            return foundStudentGroup.ToActionResult();
        }

        /// <summary>
        /// Gets all homeworks of student group
        /// </summary>
        [SwaggerResponse(200, type: typeof(List<HomeworkDto>))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet("{id}/homeworks")]
        public async Task<ActionResult> GetHomeworksOfStudentGroup(long id)
        {
            var results = await _homeworkService.GetHomeworksByLessonId(id);

            return results.ToActionResult();
        }

        /// <summary>
        /// Merges student groups
        /// </summary>
        /// <response code="200">Successful merging</response>
        [SwaggerResponse(200, type: typeof(StudentGroupDto))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpPost("merge")]
        public async Task<ActionResult<StudentGroupDto>> MergeStudentGroups([FromBody] MergeStudentGroupsDto groupsToMerge)
        {
            var result = await _studentGroupService.MergeStudentGroupsAsync(groupsToMerge);

            return result.ToActionResult();
        }
    }
}
