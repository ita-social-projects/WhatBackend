using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Course;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IActionResult> AllCourses()
        {
            var mentor = await _courseService.GetAllCoursesAsync();

            return View(mentor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateCourse(long id, UpdateCourseDto updateCourseDto)
        {
            await _courseService.UpdateCourse(id, updateCourseDto);

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
