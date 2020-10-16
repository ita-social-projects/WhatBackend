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
        #region
        private readonly ILessonService _lessonService;
        #endregion

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
            try
            {
                var lessons = await _lessonService.GetAllLessonsAsync();

                return Ok(lessons);
            }
            catch 
            { 
                return StatusCode(500); 
            }
        }

        [Authorize(Roles = "1, 2")]
        [HttpGet("students/{id}")]
        public async Task<ActionResult<List<StudentLessonModel>>> GetStudentLessons(long id)
        {
            try
            {
                var lessons = await _lessonService.GetStudentLessonsAsync(id);

                return Ok(lessons);
            }
            catch
            { 
                return StatusCode(500); 
            }
        }

        [Authorize(Roles = "2")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutLesson(long id, UpdateLessonModel lessonModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                lessonModel.Id = id;

                var updatedLesson = await _lessonService.UpdateLessonAsync(lessonModel);

                if (updatedLesson != null)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(409, "Cannot update.");
                }
            }
            catch 
            { 
                return StatusCode(500); 
            }
        }
    }
}
