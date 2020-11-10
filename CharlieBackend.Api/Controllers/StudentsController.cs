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

        [Authorize(Roles = "Admin")]
        [HttpPost("{accountId}")]
        public async Task<ActionResult> PostStudent(long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdStudentModel = await _studentService
                    .CreateStudentAsync(accountId);

            if (createdStudentModel == null)
            {
                return Result<StudentDto>.Error(ErrorCode.UnprocessableEntity,
                    "Cannot create student.").ToActionResult();
            }

            return createdStudentModel.ToActionResult();
        }
        
        [Authorize(Roles = "Mentor, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<StudentDto>>> GetStudentById(long id)
        {

            var studentModel = await _studentService.GetStudentByIdAsync(id);

            if (studentModel != null)
            {
                return Ok(new
                {
                    first_name = studentModel.FirstName,
                    last_name = studentModel.LastName,
                    //student_group_ids = studentModel.StudentGroupIds, // TODO fix
                    email = studentModel.Email
                });
            }

            return StatusCode(409, "Cannot find student with such id.");
        }

        [Authorize(Roles = "Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
        {

            var studentsModels = await _studentService.GetAllStudentsAsync();

            return Ok(studentsModels);

        }

        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut("{studentId}")]
        public async Task<ActionResult> PutStudent(long studentId, UpdateStudentDto studentModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }           

            var updatedStudent = await _studentService.UpdateStudentAsync(studentId, studentModel);

            if (updatedStudent == null)
            {
                return Result<StudentDto>.Error(ErrorCode.UnprocessableEntity,
                    "Cannot update student.").ToActionResult();
            }

            return updatedStudent.ToActionResult();
        }

        [Authorize(Roles = "Mentor, Admin")]
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
