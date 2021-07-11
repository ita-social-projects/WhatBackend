using AutoMapper;
using CharlieBackend.AdminPanel.Models.EventOccurrence;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.Core.DTO.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class EventOccurrenceController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;

        public EventOccurrenceController(IScheduleService scheduleService, IMapper mapper)
        {
            _scheduleService = scheduleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> AllEventOccurrences()
        {
            var allEventOccurences = await _scheduleService.GetAllEventOccurrences();

            var result = new List<EventOccurrenceViewModel>();

            foreach (var item in allEventOccurences)
            {
                result.Add(MapDTOtoViewModel(item));
            }

            return View(result);
        }

        private EventOccurrenceViewModel MapDTOtoViewModel(EventOccurrenceDTO eventOccurrenceDTO)
        {
            return new EventOccurrenceViewModel {
                Id = eventOccurrenceDTO.Id,
                Storage = eventOccurrenceDTO.Storage,
                StudentGroupId = eventOccurrenceDTO.StudentGroupId,
                EventStart = eventOccurrenceDTO.EventStart,
                EventFinish = eventOccurrenceDTO.EventFinish
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetEventOccurrenceById(long id)
        {
            var eventOccurence = await _scheduleService.GetEventOccurrenceById(id);

            return View(MapDTOtoViewModel(eventOccurence));
        }

        [HttpGet]
        public async Task<IActionResult> CreateEventOccurrence()
        {
            var eventOccurrenceData = await _scheduleService.PrepareStudentGroupAddAsync();

            ViewBag.EventOccurrence = eventOccurrenceData;

            return View("AddEventOccurrence");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PrepareEventOccurrenceForUpdate(long id)
        {
            var eventOccurrenceData = await _scheduleService.PrepareStudentGroupAddAsync();

            ViewBag.EventOccurrence = eventOccurrenceData;
            ViewBag.CurrentId = id;

            return View("UpdateEventOccurrence");
        }

        [HttpPost]
        public async Task<IActionResult> AddEventOccurrence(CreateScheduleDto scheduleDTO)
        {

            //Daily case
            /*scheduleDTO.Pattern.Type = Core.Entities.PatternType.Daily;
            scheduleDTO.Pattern.Interval = 2;*/

            //Weekly case
            /*scheduleDTO.Pattern.Type = Core.Entities.PatternType.Weekly;
            scheduleDTO.Pattern.Interval = 2;
            scheduleDTO.Pattern.DaysOfWeek = new List<DayOfWeek>();
            scheduleDTO.Pattern.DaysOfWeek.Add(DayOfWeek.Monday);
            scheduleDTO.Pattern.DaysOfWeek.Add(DayOfWeek.Friday);*/

            //AbsoluteMonthly case
            /*scheduleDTO.Pattern.Type = Core.Entities.PatternType.AbsoluteMonthly;
            scheduleDTO.Pattern.Interval = 2;
            scheduleDTO.Pattern.Dates = new List<int>() {15};*/

            //RelativeMonthly case
            /*scheduleDTO.Pattern.Type = Core.Entities.PatternType.RelativeMonthly;
            scheduleDTO.Pattern.Interval = 2;
            scheduleDTO.Pattern.DaysOfWeek = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Friday };
            scheduleDTO.Pattern.Index = MonthIndex.Second;*/


            await _scheduleService.CreateScheduleAsync(scheduleDTO);

            return RedirectToAction("AllEventOccurrences", "EventOccurrence");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateEventOccurrence(long id, CreateScheduleDto scheduleDTO)
        {
            await _scheduleService.UpdateScheduleByIdAsync(id, scheduleDTO);

            return RedirectToAction("AllEventOccurrences", "EventOccurrence");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteEventOccurrence(long id)
        {
            await _scheduleService.DeleteScheduleByIdAsync(id);

            return RedirectToAction("AllEventOccurrences", "EventOccurrence");
        }
    }
}
