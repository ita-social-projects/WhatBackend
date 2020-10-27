using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.Models.Course;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.DTO.Course;
using AutoMapper;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
      
        private readonly ICourseService _coursesService;
        private readonly IMapper _mapper;

        public CoursesController(ICourseService coursesService, IMapper mapper)
        {
            _coursesService = coursesService;
            _mapper = mapper;
        }

        [Authorize(Roles = "4")]
        [HttpPost]
        public async Task<ActionResult> PostCourse(CreateCourseDto courseDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var isCourseNameTaken = await _coursesService.IsCourseNameTakenAsync(courseDto.Name);

            if (isCourseNameTaken)
            {
                return StatusCode(409, "Course already exists!");
            }

            var createdCourse = await _coursesService.CreateCourseAsync(_mapper.Map<CreateCourseModel>(courseDto));

            if (createdCourse == null)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize(Roles = "2, 4")]
        [HttpGet]
        public async Task<ActionResult<IList<CourseDto>>> GetAllCourses()
        {

            var courses =  await _coursesService.GetAllCoursesAsync();

            var dtoResult = new List<CourseDto>();

            foreach (var item in courses)
            {
                dtoResult.Add(_mapper.Map<CourseDto>(item));
            }

            return Ok(dtoResult);
        }

        [Authorize(Roles = "4")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCourse(long id, UpdateCourseDto courseDto)
        {
            //if (id != courseModel.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var updatedCourse = await _coursesService.UpdateCourseAsync(id, _mapper.Map<UpdateCourseModel>(courseDto));

            if (updatedCourse != null)
            {
                return NoContent();
            }

            return StatusCode(409, "Course already exists!");
        }
    }
}
