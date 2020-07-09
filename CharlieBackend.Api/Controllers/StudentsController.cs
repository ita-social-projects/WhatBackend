using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;

        public StudentsController(IStudentService studentService, IAccountService accountService)
        {
            _studentService = studentService;
            _accountService = accountService;
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<ActionResult> PostStudent(CreateStudentModel studentModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isEmailTaken = await _accountService.IsEmailTakenAsync(studentModel.Email);
            if (isEmailTaken) return StatusCode(409, "Account already exists!");

            var createdStudentModel = await _studentService.CreateStudentAsync(studentModel);
            if (createdStudentModel == null) return StatusCode(422, "Invalid courses.");

            return Ok();
        }

        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<List<StudentModel>>> GetAllStudents()
        {
            try
            {
                var studentsModels = await _studentService.GetAllStudentsAsync();
                return Ok(studentsModels);
            }
            catch { return StatusCode(500); }
        }

        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(long id, UpdateStudentModel mentorModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var isEmailChangableTo = await _accountService.IsEmailChangableToAsync(mentorModel.Email);
                if (!isEmailChangableTo) return StatusCode(409, "Email is already taken!");

                mentorModel.Id = id;
                var updatedCourse = await _studentService.UpdateStudentAsync(mentorModel);
                if (updatedCourse != null) return NoContent();
                else return StatusCode(409, "Cannot update.");

            }
            catch { return StatusCode(500); }
        }
    }
}