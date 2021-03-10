using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
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

        [HttpGet]
        public async Task<IActionResult> AllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            return View(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateCourse(long id, UpdateCourseDto updateCourseDto)
        {
            await _courseService.UpdateCourse(id, updateCourseDto);

            return RedirectToAction("AllCourses", "Courses");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EnableCourse(long id)
        {
            await _courseService.EnableCourseAsync(id);

            return RedirectToAction("AllCourses", "Courses");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableCourse(long id)
        {
            await _courseService.DisableCourseAsync(id);

            return RedirectToAction("AllCourses", "Courses");
        }
    }
}
