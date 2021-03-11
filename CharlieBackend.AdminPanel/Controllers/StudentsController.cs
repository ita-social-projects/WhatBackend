using System.Threading.Tasks;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;

        }

        public async Task<IActionResult> AllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();

            return View(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateStudent(long id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            ViewBag.Student = student;

            return View("UpdateStudent");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStudent(long id, UpdateStudentDto data)
        {
            var updatedStudent = await _studentService.UpdateStudentAsync(id, data);

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(long id)
        {
            var addedStudent = await _studentService.AddStudentAsync(id);

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableStudent(long id)
        {
            var disabledStudent = await _studentService.DisableStudentAsync(id);

            return RedirectToAction("AllStudents", "Students");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EnableStudent(long id)
        {
            await _studentService.EnableStudentAsync(id);

            return RedirectToAction("AllStudents", "Students");
        }
    }
}
