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

            foreach(var item in allEventOccurences)
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

        [HttpGet]
        public async Task<IActionResult> AddEventOccurrence(CreateScheduleDto scheduleDTO)
        {
            await _scheduleService.CreateScheduleAsync(scheduleDTO);

            return RedirectToAction("AllEventOccurrences", "EventOccurrence");
        }

        [HttpPut]
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
