using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [Authorize(Roles = "2")]
        [HttpPost]
        public async Task<ActionResult> PostLesson(CreateLessonModel lessonModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (HttpContext.Items["mentorId"] == null)
            {
                return BadRequest("Need to sign in.");
            }

//            long.TryParse
            var createdLesson = await _lessonService.CreateLessonAsync(lessonModel, Convert.ToInt64(HttpContext.Items["mentorId"].ToString()));

            if (createdLesson == null)
            {
                return StatusCode(422, "Cannot create lesson");
            }

            return Ok();
        }

        [Authorize(Roles = "2")]
        [HttpGet]
        public async Task<ActionResult<List<LessonModel>>> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();

            return Ok(lessons);
        }

        [Authorize(Roles = "1, 2")]
        [HttpGet("students/{id}")]
        public async Task<ActionResult<List<StudentLessonModel>>> GetStudentLessons(long id)
        {

            var lessons = await _lessonService.GetStudentLessonsAsync(id);

            return Ok(lessons);
        }

        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutLesson(long id, UpdateLessonModel lessonModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            lessonModel.Id = id;

            var updatedLesson = await _lessonService.UpdateLessonAsync(lessonModel);

            if (updatedLesson != null)
            {
                return NoContent();
            }

            return StatusCode(409, "Cannot update.");

        }
    }
}
