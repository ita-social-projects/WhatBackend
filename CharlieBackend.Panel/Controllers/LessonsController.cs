using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Helpers;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


            //var id = User.Claims.FirstOrDefault(c => c.Type == "AccountId").Value;

            //var enti = User.Claims.FirstOrDefault(c => c.Type == ClaimConstants.IdClaim).Value;

            //var mail = User.Claims.FirstOrDefault(c => c.Type == ClaimConstants.EmailClaim).Value;

            var Id = _currentUserService.AccountId;
            var role = _currentUserService.Role;
            var entity = _currentUserService.EntityId;
            var mails = _currentUserService.Email;
            

            //var user = User.Identity as ClaimsIdentity;
            //if (user != null)
            //{
            //    var userClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            //}
            //if (User.IsInRole("Mentor"))
            //{
            var filteredM = new FilterLessonsRequestDto()
            {
                StartDate = DateTime.Now.AddDays(-30)
            };

            var sel = await _lessonService.GetLessonsForMentorAsync(filteredM);
                return View(sel);

            //}
            //var lessons = await _lessonService.GetLessonsByDate();
            //return View(lessons);
        }

        [HttpGet()]
        public async Task<IActionResult> SelectGroup()
        {
            IEnumerable<StudentGroupViewModel> stGroups = await _studentGroupService.GetAllStudentGroupsAsync();
            //IEnumerable<StudentGroupViewModel> groups = stGroups.Where(x => x.Course.Id == courseId).ToList();
            return View("GroupSelection", stGroups);
        }

        [HttpGet]
        public async Task<IActionResult> CreateLesson(long stGroupId)
        {
            var LessonData = await _lessonService.PrepareLessonAddAsync(stGroupId);

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

    }
}
