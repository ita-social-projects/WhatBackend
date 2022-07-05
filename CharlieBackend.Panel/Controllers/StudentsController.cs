using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Panel.Models.Languages;
using CharlieBackend.Panel.Models.Students;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor")]
    [Route("[controller]/[action]")]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<StudentsController> _stringLocalizer;

        public StudentsController(IStudentService studentService, IStringLocalizer<StudentsController> stringLocalizer)
        {
            _studentService = studentService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> AllStudents()
        {
            if (Languages.language == Language.UA)
                Languages.language = Language.EN;
            else
                Languages.language = Language.UA;

            StudentLocalizationViewModel studentLocalizationViewModel = new StudentLocalizationViewModel
            {
                StudentViews = await _studentService.GetAllStudentsAsync(),
                StringLocalizer = _stringLocalizer
            };

            return View(studentLocalizationViewModel);
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
