using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Models.Lesson;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [Route("api/lesson")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost]
        public async Task<ActionResult<LessonModel>> PostLesson(LessonModel lessonModel)
        {
            return StatusCode(501);
            // TODO: implement StudentGroup and Mentor. Unless they are not implemented, we can't use LessonController
            if (!ModelState.IsValid) return BadRequest();

            var createdLesson = await _lessonService.CreateLessonAsync(lessonModel);
            if (createdLesson == null) return StatusCode(500);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<LessonModel>>> GetAllLessons()
        {
            try
            {
                var lessons = await _lessonService.GetAllLessonsAsync();
                return Ok(lessons);
            }
            catch { return StatusCode(500); }
        }
    }
}
