using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage courses data
    /// </summary>
    [Route("api/v{version:apiVersion}/courses")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _coursesService;

        /// <summary>
        /// Courses controllers constructor
        /// </summary>
        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

        /// <summary>
        /// Adds new course
        /// </summary>
        /// <response code="200">Course succeesfully added</response>
        /// <response code="HTTP: 409, API: 5">Course already exists</response>
        [SwaggerResponse(200, type: typeof(CourseDto))]
        [Authorize(Roles = "Admin, Secretary")]
        [HttpPost]
        public async Task<ActionResult<CreateCourseDto>> PostCourse(CreateCourseDto courseDto)
        {
            var createdCourse = await _coursesService.CreateCourseAsync(courseDto);

            return createdCourse.ToActionResult();
        }

        /// <summary>
        /// Gets courses
        /// </summary>
        /// <param name="isActive">
        /// 1. If IsActive is true – endpoint returns all active Courses.
        /// 2. If IsActive is false – endpoint returns all not active Courses.
        /// 3. If IsActive is null – endpoint returns all courses. </param>
        /// <response code="200">Successful return of list of courses</response>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        public async Task<IList<CourseDto>> GetCourses(bool? isActive)
        {
            var courses = await _coursesService.GetCoursesAsync(isActive);

            return courses;
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <response code="200">Successful update of course</response>
        /// <response code="HTTP: 400, API: 0">Bad request</response>
        /// <response code="HTTP: 409, API: 5">Course already exist</response>
        [Authorize(Roles = "Admin, Secretary")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> PutCourse(long id, UpdateCourseDto courseDto)
        {
            var updatedCourse = await _coursesService.UpdateCourseAsync(id, courseDto);

            return updatedCourse.ToActionResult();

        }

        /// <summary>
        /// Disable course
        /// </summary>
        /// <response code="200">Course successfully disabled</response>
        /// <response code="HTTP: 400, API: 0">Course has active student group</response>
        [Authorize(Roles = "Admin, Secretary")]
        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseDto>> DisableCourseV2(long id)
        {
            var disableCourse = await _coursesService.DisableCourseAsync(id);

            return disableCourse.ToActionResult();
        }

        /// <summary>
        /// Enable course
        /// </summary>
        /// <response code="200">Course successfully enabled</response>
        /// <response code="HTTP: 409, API: 5">Course is already active</response>
        [Authorize(Roles = "Admin, Secretary")]
        [MapToApiVersion("2.0")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<CourseDto>> EnableCourseV2(long id)
        {
            var enableCourse = await _coursesService.EnableCourseAsync(id);

            return enableCourse.ToActionResult();
        }

        /// <summary>
        /// Disable course
        /// </summary>
        /// <response code="200">Course successfully disabled</response>
        /// <response code="HTTP: 400, API: 0">Course has active student group</response>
        [Authorize(Roles = "Admin, Secretary")]
        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DisableCourse(long id)
        {
            var disableCourse = await _coursesService.DisableCourseAsync(id);

            Result<bool> result = new Result<bool>();

            if (disableCourse.Error == null)
            {
                result.Data = true;
            }
            else
            {
                result.Error = disableCourse.Error;
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Enable course
        /// </summary>
        /// <response code="200">Course successfully enabled</response>
        /// <response code="HTTP: 409, API: 5">Course is already active</response>
        [Authorize(Roles = "Admin, Secretary")]
        [MapToApiVersion("1.0")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<bool>> EnableCourse(long id)
        {
            var enableCourse = await _coursesService.EnableCourseAsync(id);

            Result<bool> result = new Result<bool>();

            if (enableCourse.Error == null)
            {
                result.Data = true;
            }
            else
            {
                result.Error = enableCourse.Error;
            }

            return result.ToActionResult();
        }
    }
}
