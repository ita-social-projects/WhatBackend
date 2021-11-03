using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin, Secretary, Mentor")]
    [Route("[controller]/[action]")]
    public class LessonsController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly IMentorService _mentorService;

        public LessonsController(ILessonService lessonService, IMentorService mentorService)
        {
            _lessonService = lessonService;
            _mentorService = mentorService;
        }

        [HttpGet]
        public async Task<IActionResult> AllLessons()
        {
            if (User.IsInRole("Mentor"))
            {
                var filteredM = new FilterLessonsRequestDto()
                {
                    StartDate = DateTime.Now.AddDays(-30)
                };

                var sel = await _lessonService.GetLessonsForMentorAsync(filteredM);
                return View(sel);

            }
            var lessons = await _lessonService.GetLessonsByDate();
            return View(lessons);
        }



        [HttpGet]
        public async Task<IActionResult> CreateLesson()
        {
            var LessonData = await _lessonService.PrepareLessonAddAsync();

            ViewBag.Lesson = LessonData;

            return View("AddLesson", LessonData);
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(LessonCreateViewModel data)
        {
            //data.LessonVisits = new List<VisitDto>(5);
            //foreach (var item in data.LessonVisits)
            //{
            //    item.StudentId = 5;
            //    item.Presence = true;
            //}
            await _lessonService.AddLessonEndpoint(data);

            return RedirectToAction("AllLessons", "Lesson");
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
            var events = await _lessonService.Get2MounthEvents();
            return View("Events", events);
        }

        //public async Task<IActionResult> VisitsPartial(long id)
        //{
        //    var students = await _lessonService.StudentsPartial(5);
        //    ViewBag.Students = students;

        //    return PartialView("VisitsPartial");

        //}
    }
}
