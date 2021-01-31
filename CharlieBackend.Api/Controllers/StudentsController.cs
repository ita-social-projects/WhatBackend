using System;
using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Api.SwaggerExamples.StudentsController;
using CharlieBackend.Core.DTO.Lesson;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage students
    /// </summary>
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        #region
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;
        private readonly ILessonService _lessonService;
        #endregion
        /// <summary>
        /// Students Controllers constructor
        /// </summary>
        public StudentsController(IStudentService studentService, 
            IAccountService accountService, ILessonService lessonService)
        {
            _studentService = studentService;
            _accountService = accountService;
            _lessonService = lessonService;
        }

        /// <summary>
        /// Addition of new student
        /// </summary>
        /// <response code="200">Successful assing of account into student</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find account</response>
        /// <response code="HTTP: 400, API: 0">Error, account already assigned</response>
        [SwaggerResponse(200, type: typeof(StudentDto))]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpPost("{accountId}")]
        public async Task<ActionResult> PostStudent(long accountId)
        {
            var createdStudentModel = await _studentService
                    .CreateStudentAsync(accountId);

            return createdStudentModel.ToActionResult();
        }

        /// <summary>
        /// Get student information by student id
        /// </summary>
        /// <response code="200">Successful return of student</response>
        /// <response code="409">Error, can not find student</response>
        [SwaggerResponse(200, type: typeof(StudentMock))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(long id)
        {

            var studentModelResult = await _studentService.GetStudentByIdAsync(id);

            if (studentModelResult != null)
            {
                return studentModelResult.ToActionResult(); 
            }

            return StatusCode(409, "Cannot find student with such id.");
        }

        /// <summary>
        /// Get all students (active and inactive)
        /// </summary>
        /// <response code="200">Successful return of students list</response>
        [SwaggerResponse(200, type: typeof(IList<StudentDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet]
        public async Task<ActionResult<IList<StudentDto>>> GetAllStudents() 
        {

            var studentsModelsResult = await _studentService.GetAllStudentsAsync();

            return studentsModelsResult.ToActionResult();
        }

        /// <summary>
        /// Get only active students
        /// </summary>
        /// <response code="200">Successful return of students list</response>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("active")]
        public async Task<ActionResult<IList<StudentDto>>> GetAllActiveStudents()
        {

            var studentsModelsResult = await _studentService.GetAllActiveStudentsAsync();

            return studentsModelsResult.ToActionResult();
        }

        /// <summary>
        /// Gets all of the student's study group
        /// </summary>
        /// <response code="200">Successful return of student's study groups</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find student or student's study groups</response>
        [SwaggerResponse(200, type: typeof(IList<StudentStudyGroupsDto>))]
        [Authorize(Roles = "Secretary, Mentor, Admin")]
        [HttpGet("{id}/groups")]
        public async Task<ActionResult<IList<StudentStudyGroupsDto>>> GetStudentStudyGroupsByStudentId(long id)
        {
            var foundGroups = await _studentService
                    .GetStudentStudyGroupsByStudentIdAsync(id);

            return foundGroups.ToActionResult();
        }

        /// <summary>
        /// Updates student
        /// </summary>
        /// <response code="200">Successful update of student</response>
        /// <response code="HTTP: 404, API: 3">Error, can not find student</response>
        /// <response code="HTTP: 400, API: 0">Error, update data is wrong</response>
        [SwaggerResponse(200, type: typeof(UpdateStudentDto))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPut("{studentId}")]
        public async Task<ActionResult> PutStudent(long studentId, [FromBody]UpdateStudentDto studentModel)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(studentId, studentModel);

            return updatedStudent.ToActionResult();
        }

        /// <summary>
        /// Disabling of student
        /// </summary>
        /// <response code="204">Successful update of student</response>
        /// <response code="400">Error, student not found</response>
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DisableStudent(long id)
        {
            var disabledStudentModel = await _studentService.DisableStudentAsync(id);

            return disabledStudentModel.ToActionResult();
        }

        /// <summary>
        /// Get filter list of lessons for student
        /// </summary>
        /// <response code="200">Returned filtered list of lessons for student </response>
        [SwaggerResponse(200, type: typeof(IList<LessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPost("lessons")]
        public async Task<IList<LessonDto>> GetLessonsForStudent([FromBody] FilterLessonsRequestDto filterModel)
        {
            var context = HttpContext.User;
            var lessons = await _lessonService.GetLessonsForStudentAsync(filterModel, context);

            return lessons;
        }
    }
}
