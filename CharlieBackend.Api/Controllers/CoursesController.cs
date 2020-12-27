﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core;
using CharlieBackend.Core.DTO.Homework;

namespace CharlieBackend.Api.Controllers
{
    /// <summary>
    /// Controller to manage cources data
    /// </summary>
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;
        private readonly ICourseService _coursesService;

        /// <summary>
        /// Courses controllers constructor
        /// </summary>
        public CoursesController(ICourseService coursesService, IHomeworkService homeworkService)
        {
            _coursesService = coursesService;
            _homeworkService = homeworkService;
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
        /// Gets all cources
        /// </summary>
        /// <response code="200">Successful return of list of courses</response>
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet]
        public async Task<IList<CourseDto>> GetAllCourses()
        {
            var courses = await _coursesService.GetAllCoursesAsync();

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
        /// Delete course
        /// </summary>
        /// <response code="200">Successful delete  course</response>
        /// <response code="HTTP: 400, API: 0">Bad request</response>
        [Authorize(Roles = "Admin, Secretary")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseDto>> DisableCourse(long id)
        {
            var disableCourse = await _coursesService.DisableCourceAsync(id);

            return disableCourse.ToActionResult();
        }

        /// <summary>
        /// Gets all homeworks of course
        /// </summary>
        [SwaggerResponse(200, type: typeof(List<HomeworkDto>))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpGet("{id}/homeworks")]
        public async Task<ActionResult> GetHomeworksOfCourse(long id)
        {
            var results = await _homeworkService
                        .GetHomeworksOfCourseAsync(id);

            return results.ToActionResult();
        }
    }
}
