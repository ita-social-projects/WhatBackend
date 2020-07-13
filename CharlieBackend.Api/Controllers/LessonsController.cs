using CharlieBackend.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        //[Authorize(Roles = "2")]
        //[HttpPost]
        //public async Task<ActionResult<LessonModel>> PostLesson(LessonModel lessonModel)
        //{
        //    return StatusCode(501);
        //    // TODO: implement StudentGroup and Mentor. Unless they are not implemented, we can't use LessonController
        //    if (!ModelState.IsValid) return BadRequest();

        //    var createdLesson = await _lessonService.CreateLessonAsync(lessonModel);
        //    if (createdLesson == null) return StatusCode(500);

        //    return Ok();
        //}

        //[Authorize(Roles = "2")]
        //[HttpGet]
        //public async Task<ActionResult<List<LessonModel>>> GetAllLessons()
        //{
        //    try
        //    {
        //        var lessons = await _lessonService.GetAllLessonsAsync();
        //        return Ok(lessons);
        //    }
        //    catch { return StatusCode(500); }
        //}
    }
}
