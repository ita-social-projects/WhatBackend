using System;
using CharlieBackend.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Business.Services.Interfaces;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        #region
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;
        #endregion

        public StudentsController(IStudentService studentService, 
            IAccountService accountService)
        {
            _studentService = studentService;
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin, Secretary")]
        [HttpPost("{accountId}")]
        public async Task<ActionResult> PostStudent(long accountId)
        {
            var createdStudentModel = await _studentService
                    .CreateStudentAsync(accountId);

            return createdStudentModel.ToActionResult();
        }
        
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(long id)
        {

            var studentModel = await _studentService.GetStudentByIdAsync(id);

            if (studentModel != null)
            {
                return Ok(studentModel); //student_group_ids = studentModel.StudentGroupIds, // TODO fix
          
            }

            return StatusCode(409, "Cannot find student with such id.");
        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet]
        public async Task<ActionResult<IList<StudentDto>>> GetAllStudents() // returns all students (active and unactive)
        {

            var studentsModels = await _studentService.GetAllStudentsAsync();

            return Ok(studentsModels);

        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet("active")]
        public async Task<ActionResult<IList<StudentDto>>> GetAllActiveStudents() // returns only active students
        {

            var studentsModels = await _studentService.GetAllActiveStudentsAsync();

            return Ok(studentsModels);

        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpPut("{studentId}")]
        public async Task<ActionResult> PutStudent(long studentId, UpdateStudentDto studentModel)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(studentId, studentModel);

            return updatedStudent.ToActionResult();
        }

        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DisableStudent(long id)
        {

            var accountId = await _studentService.GetAccountId(id);

            if (accountId == null)
            {
                return BadRequest("Unknown student id.");
            }

            var isDisabled = await _accountService
                    .DisableAccountAsync((long)accountId);

            if (isDisabled)
            {
                return NoContent();
            }

            return StatusCode(500, "Error occurred while trying to disable student account.");
        }
    }
}
