using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
      
        private readonly ICourseService _coursesService;

        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CreateCourseDto>> PostCourse(CreateCourseDto courseDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isCourseNameTaken = await _coursesService.IsCourseNameTakenAsync(courseDto.Name);

            if (isCourseNameTaken)
            {
                return StatusCode(409, "Course already exists!");
            }

            var createdCourse = await _coursesService.CreateCourseAsync(courseDto);

            if (createdCourse == null)
            {
                return StatusCode(500);
            }

            return Ok(createdCourse);
        }

        [Authorize(Roles = "Mentor, Admin")]
        [HttpGet]
        public async Task<ActionResult<IList<CourseDto>>> GetAllCourses()
        {

            var courses =  await _coursesService.GetAllCoursesAsync();

            return Ok(courses);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> PutCourse(long id, UpdateCourseDto courseDto)
        {
            //if (id != courseModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var updatedCourse = await _coursesService.UpdateCourseAsync(id, courseDto);

            if (updatedCourse != null)
            {
                return Ok(updatedCourse);
            }

            return StatusCode(409, "Course already exists!");
        }
    }
}
