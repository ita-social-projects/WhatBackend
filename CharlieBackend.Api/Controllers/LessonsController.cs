using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CharlieBackend.Core.DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using CharlieBackend.Business.Services.Interfaces;

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

        [Authorize(Roles = "Admin, Mentor,")]
        [HttpGet]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();

            return Ok(lessons);
        }

        [Authorize(Roles = "Admin, Mentor, Secretary, Student")]
        [HttpGet("students/{id}")]
        public async Task<ActionResult<List<StudentLessonDto>>> GetStudentLessons(long id)
        {

            var lessons = await _lessonService.GetStudentLessonsAsync(id);

            return Ok(lessons);
        }

        [Authorize(Roles = "Admin, Mentor")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutLesson(long id, UpdateLessonDto lessonDto)
        {
            var updatedLesson = await _lessonService.UpdateLessonAsync(id, lessonDto);

            if (updatedLesson != null)
            {
                return Ok(updatedLesson);
            }

            return StatusCode(409, "Cannot update.");

        }
    }
}
