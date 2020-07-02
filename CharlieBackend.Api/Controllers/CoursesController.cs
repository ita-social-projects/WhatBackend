using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _coursesService;

        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpPost]
        public async Task<ActionResult> PostCourse(CourseModel courseModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var createdCourse = await _coursesService.CreateCourseAsync(courseModel);
            if (createdCourse == null) return StatusCode(500);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseModel>>> GetAllCourses()
        {
            try
            {
                var courses = await _coursesService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch { return StatusCode(500); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCourse(long id, CourseModel courseModel)
        {
            if (id != courseModel.Id) return BadRequest();

            try {
                var updatedCourse = await _coursesService.UpdateCourseAsync(courseModel);
                if (updatedCourse != null) return NoContent();
                else return StatusCode(422);

            } catch { return StatusCode(500); }
        }
    }
}
