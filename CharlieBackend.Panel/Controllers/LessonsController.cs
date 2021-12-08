using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor, Student")]
    [Route("[controller]/[action]")]
    public class LessonsController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly IMentorService _mentorService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly ICurrentUserService _currentUserService;

        public LessonsController(ILessonService lessonService, IMentorService mentorService, IStudentGroupService studentGroupService, 
            ICurrentUserService currentUserService)
        {
            _lessonService = lessonService;
            _mentorService = mentorService;
            _studentGroupService = studentGroupService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> AllLessons()
        {
            var role = _currentUserService.Role;
            var entity = _currentUserService.EntityId;

            if (role == Core.Entities.UserRole.Mentor)
            {
                var lessons = await _lessonService.GetAllLessonsForMentor(entity);
                return View(lessons);
            }
            if (role == Core.Entities.UserRole.Student)
            {
                var lessons = await _lessonService.GetStudentLessonsAsync(entity);
                return View("AllStudentsLessons", lessons);
            }

            var allLessons = await _lessonService.GetLessonsByDate();
            return View(allLessons.OrderByDescending(x => x.LessonDate));
        }

        [HttpGet()]
        public async Task<IActionResult> SelectGroup()
        {
            IEnumerable<StudentGroupViewModel> stGroups = await _studentGroupService.GetAllStudentGroupsAsync();
            return View("GroupSelection", stGroups);
        }

        [HttpGet]
        public async Task<IActionResult> CreateLesson(long stGroupId, long? eventId)
        {
            if (User.IsInRole("Mentor"))
            {
                ViewBag.MentorId = _currentUserService.EntityId;
            }
            var LessonData = await _lessonService.PrepareLessonAddAsync(stGroupId);

            ViewBag.EventId = eventId;

            ViewBag.Lesson = LessonData;

            return View("AddLesson", LessonData);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(LessonCreateViewModel data)
        {
            await _lessonService.AddLessonEndpoint(data);

            return RedirectToAction("AllLessons", "Lessons");
        }

        [HttpGet]
        public async Task<IActionResult> Attendance(long id)
        {
            var visits = await _lessonService.LessonVisits(id);
            return View(visits);
        }

        [HttpGet]
        public async Task<IActionResult> RangeOfEvents()
        {
            var events = await _lessonService.Get2DaysEvents();
            return View("Events", events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PrepareLessonForUpdate(long id)
        {
            var lesson = await _lessonService.PrepareLessonUpdateAsync(id);

            ViewBag.Lesson = lesson;

            return View("UpdateLesson", lesson);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateLesson(long id, LessonUpdateViewModel data)
        {
            await _lessonService.UpdateLessonEndpoint(id, data);

            return RedirectToAction("AllLessons", "Lessons");
        }

    }
}
