using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using CharlieBackend.Library.SwaggerExamples.LessonsController;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/lessons")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        /// <summary>
        /// Adds new lesson
        /// </summary>
        /// <remarks>
        /// In body of request **id** is not recessary
        /// </remarks>
        /// <response code="200">Successful addition of the lesson</response>
        /// <response code="422">Can not create lesson</response>
        /// <response code="500">Can not create lesson</response>
        [SwaggerResponse(200, type: typeof(LessonDto))]
        [Authorize(Roles = "Mentor, Admin")]
        [HttpPost]
        public async Task<ActionResult> PostLesson(CreateLessonDto lessonDto)
        {
            var createdLesson = await _lessonService.CreateLessonAsync(lessonDto);
            
            if (createdLesson == null)
            {
                return StatusCode(422, "Cannot create lesson");
            }

            return Ok(createdLesson);
        }

        /// <summary>
        /// Assinging mentor to lesson
        /// </summary>
        /// <response code="200">Successful assinging mentor to lesson</response>
        /// <response code="422">Error, lesson does not exist</response>
        /// <response code="500">Can not assign mentor to lesson</response>
        [SwaggerResponse(200, type: typeof(Lesson))]
        [Authorize(Roles = "Admin, Secretary")]
        [Route("assign")]
        [HttpPost]
        public async Task<ActionResult> AssignMentorToLesson(AssignMentorToLessonDto ids)
        {
            var changedLesson = await _lessonService.AssignMentorToLessonAsync(ids);

            if (changedLesson == null)
            {
                return StatusCode(422, "Lesson doesn't exist");
            }
            return Ok(changedLesson);
        }

        /// <summary>
        /// Gets list of all lessons
        /// </summary>
        /// <response code="200">Successful return of lessons list</response>
        /// <response code="500">Error while getting list of lessons</response>
        [SwaggerResponse(200, type: typeof(List<LessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary")]
        [HttpGet]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();

            return Ok(lessons);
        }

        /// <summary>
        /// Returns list of lessons of exact student
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Successful return of lessons list of given student</response>
        /// <response code="500">Error while getting list of student lessons</response>
        [SwaggerResponse(200, type: typeof(IList<StudentLessonDto>))]
        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("students/{id}")]
        public async Task<ActionResult<List<StudentLessonDto>>> GetStudentLessons(long id)
        {
            var lessons = await _lessonService.GetStudentLessonsAsync(id);

            return Ok(lessons);
        }

        /// <summary>
        /// Updates given lesson
        /// </summary>
        /// <remarks>
        /// In body of request **id** is not recessary
        /// </remarks>
        /// <response code="200">Successful update of given lesson</response>
        /// <response code="409">Can not update lesson</response>
        /// <response code="500">Error while updating lesson</response>
        [SwaggerResponse(200, type: typeof(LessonDto))]
        [Authorize(Roles = "Admin, Mentor")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutLesson(long id, UpdateLessonDto lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            lessonDto.Id = id;

            var updatedLesson = await _lessonService.UpdateLessonAsync(lessonDto);

            if (updatedLesson != null)
            {
                return Ok(updatedLesson);
            }

            return StatusCode(409, "Cannot update.");

        }
    }
}
