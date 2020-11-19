using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Library.SwaggerExamples.CoursesController;

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

        /// <summary>
        /// Adds new course
        /// </summary>
        /// <response code="200">Course succeesfully added</response>
        /// <response code="409">Course already exists</response>
        /// <response code="500">Can not create course</response>
        [SwaggerResponse(200, type: typeof(CourseDto))]
        [Authorize(Roles = "Admin, Secretary")]
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

        /// <summary>
        /// Gets all cources
        /// </summary>
        /// <response code="200">Successful return of list of courses</response>
        /// <response code="500">Unable to get courses list</response>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        public async Task<ActionResult<IList<CourseDto>>> GetAllCourses()
        {

            var courses =  await _coursesService.GetAllCoursesAsync();

            return Ok(courses);
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <response code="200">Successful update of course</response>
        /// <response code="400">Bad request</response>
        /// <response code="409">Course already exist</response>
        [Authorize(Roles = "Admin, Secretary")]
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
