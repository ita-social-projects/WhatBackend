using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Route("[controller]/[action]")]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        public async Task<IActionResult> AllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            return View(courses);
        }

        [Authorize(Roles = "Admin, Secretary")]
        [HttpPost]
        public async Task<IActionResult> AddCourse(CreateCourseDto courseDto)
        {
            await _courseService.AddCourseAsync(courseDto);

            return RedirectToAction("AllCourses", "Courses");
        }

        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateCourse(long id, UpdateCourseDto updateCourseDto)
        {
            await _courseService.UpdateCourse(id, updateCourseDto);

            return RedirectToAction("AllCourses", "Courses");
        }

        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet("{id}")]
        public async Task<IActionResult> EnableCourse(long id)
        {
            await _courseService.EnableCourseAsync(id);

            return RedirectToAction("AllCourses", "Courses");
        }

        [Authorize(Roles = "Admin, Secretary")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DisableCourse(long id)
        {
            await _courseService.DisableCourseAsync(id);

            return RedirectToAction("AllCourses", "Courses");
        }
    }
}
