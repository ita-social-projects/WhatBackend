using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.Models.Course;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;


namespace CharlieBackend.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        #region
        private readonly ICourseService _coursesService;
        #endregion

        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

        [Authorize(Roles = "4")]
        [HttpPost]
        public async Task<ActionResult> PostCourse(CreateCourseModel courseModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isCourseNameTaken = await _coursesService.IsCourseNameTakenAsync(courseModel.Name);

            if (isCourseNameTaken)
            {
                return StatusCode(409, "Course already exists!");
            }

            var createdCourse = await _coursesService.CreateCourseAsync(courseModel);

            if (createdCourse == null)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize(Roles = "2, 4")]
        [HttpGet]
        public async Task<ActionResult<List<CourseModel>>> GetAllCourses()
        {
            try
            {
                var courses = await _coursesService.GetAllCoursesAsync();

                return Ok(courses);
            }
            catch 
            { 
                return StatusCode(500); 
            }
        }

        [Authorize(Roles = "4")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCourse(long id, UpdateCourseModel courseModel)
        {
            //if (id != courseModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                courseModel.Id = id;

                var updatedCourse = await _coursesService.UpdateCourseAsync(courseModel);

                if (updatedCourse != null)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(409, "Course already exists!");
                }
            }
            catch 
            { 
                return StatusCode(500);
            }
        }
    }
}
