using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}