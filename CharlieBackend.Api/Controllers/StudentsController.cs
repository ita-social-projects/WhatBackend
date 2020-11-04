using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Core;

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
        [HttpPost("{id}")]
        public async Task<ActionResult> PostStudent(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdStudentModel = await _studentService
                    .CreateStudentAsync(id);

            if (createdStudentModel == null)
            {
                return Result<StudentDto>.Error(ErrorCode.UnprocessableEntity,
                    "Cannot create student.").ToActionResult();
            }

            return createdStudentModel.ToActionResult();
        }
        /*
        [Authorize(Roles = "2, 4")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<UpdateStudentModel>>> GetStudentById(long id)
        {

            var studentModel = await _studentService.GetStudentByIdAsync(id);

            if (studentModel != null)
            {
                return Ok(new
                {
                    first_name = studentModel.FirstName,
                    last_name = studentModel.LastName,
                    student_group_ids = studentModel.StudentGroupIds,
                    email = studentModel.Email
                });
            }

            return StatusCode(409, "Cannot find student with such id.");
        }*/

        [Authorize(Roles = "Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
        {

            var studentsModels = await _studentService.GetAllStudentsAsync();

            return Ok(studentsModels);

        }

        [Authorize(Roles = "Mentor, Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, UpdateStudentDto mentorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }           

            var updatedStudent = await _studentService.UpdateStudentAsync(id, mentorModel);

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