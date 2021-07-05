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

        [HttpPost]
        public async Task<IActionResult> CreateEventOccurrence(CreateScheduleDto scheduleDTO)
        {
            await _scheduleService.CreateSheduleAsync(scheduleDTO);

            return RedirectToAction("GetAllEventOccurrences", "EventOccurrence");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEventOccurrence(long id, CreateScheduleDto scheduleDTO)
        {
            await _scheduleService.UpdateSheduleByIdAsync(id, scheduleDTO);

            return RedirectToAction("GetAllEventOccurrences", "EventOccurrence");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEventOccurrence(long id, DateTime? startDate, DateTime? finishDate)
        {
            await _scheduleService.DeleteSheduleByIdAsync(id, startDate, finishDate);

            return RedirectToAction("GetAllEventOccurrences", "EventOccurrence");
        }
    }
}
