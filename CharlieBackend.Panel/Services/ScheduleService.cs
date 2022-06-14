using AutoMapper;
using CharlieBackend.Core.DTO.Event;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Panel.Models.EventOccurrence;
using CharlieBackend.Panel.Models.ScheduledEvent;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    /// <summary>
    /// Class that provides methods for work with schedules.
    /// </summary>
    class ScheduleService : IScheduleService
    {
        private readonly IApiUtil _apiUtil;
        private readonly ScheduleApiEndpoints _scheduleApiEndpoints;
        private readonly EventsApiEndpoints _eventsApiEndpoints;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IThemeService _themeService;
        private readonly IMentorService _mentorService;
        private readonly IMapper _mapper;

        public ScheduleService(
            IApiUtil apiUtil, 
            IOptions<ApplicationSettings> options,
            IMapper mapper,
            IMentorService mentorService,
            IStudentGroupService studentGroupService,
            IThemeService themeService)
        {
            _apiUtil = apiUtil;
            _themeService = themeService;
            _mentorService = mentorService;
            _studentGroupService = studentGroupService;
            _scheduleApiEndpoints = options.Value.Urls.ApiEndpoints.Schedule;
            _eventsApiEndpoints = options.Value.Urls.ApiEndpoints.Events;
            _mapper = mapper;
        }

        public async Task<IList<EventOccurrenceDTO>> GetAllEventOccurrences()
        {
            var result = await _apiUtil
                .GetAsync<IList<EventOccurrenceDTO>>(_scheduleApiEndpoints.EventOccurrencesEndpoint);

            return result;
        }

        public async Task<IList<ScheduledEventDTO>> GetEventsFiltered(
            ScheduledEventFilterRequestDTO scheduledEventFilterDto)
        {
            if (scheduledEventFilterDto is null)
            {
                throw new ArgumentNullException();
            }

            var result = await _apiUtil.PostAsync<IList<ScheduledEventDTO>, ScheduledEventFilterRequestDTO>(
                url: _scheduleApiEndpoints.EventsEndpoint,
                data: scheduledEventFilterDto);

            return result;
        }

        public async Task<EventOccurrenceDTO> GetEventOccurrenceById(long id)
        {
            var eventOccurence =
               string.Format(_scheduleApiEndpoints.EventOccurrenceById, id);

            var result = await _apiUtil
                .GetAsync<EventOccurrenceDTO>(eventOccurence);

            return result;
        }

        public async Task CreateScheduleAsync(CreateScheduleDto scheduleDTO)
        {
              await _apiUtil
                .CreateAsync<EventOccurrenceDTO, CreateScheduleDto>(_scheduleApiEndpoints.AddEventOccurrence, scheduleDTO);
        }

        public async Task DeleteScheduleByIdAsync(long eventOccurrenceID)
        {
            var eventOccurence =
               string.Format(_scheduleApiEndpoints.DeleteScheduleEndpoint, eventOccurrenceID);

            await _apiUtil
                .DeleteAsync<EventOccurrenceDTO>(eventOccurence);
        }

        public async Task UpdateScheduleByIdAsync(long eventOccurrenceID, CreateScheduleDto updateScheduleDto)
        {
            var eventOccurence =
               string.Format(_scheduleApiEndpoints.UpdateScheduleEndpoint, eventOccurrenceID);

            await _apiUtil
                .PutAsync<EventOccurrenceDTO, CreateScheduleDto>(eventOccurence, updateScheduleDto);
        }

        public async Task<ScheduledEventEditViewModel> PrepareSingleEventUpdateAsync(long id)
        {
            var eventEndpoint =
               string.Format(_eventsApiEndpoints.EventById, id);

            var singleEventTask = _apiUtil.GetAsync<ScheduledEventDTO>(eventEndpoint);
            var studentGroupsTask = _studentGroupService.GetAllStudentGroupsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var themesTask = _themeService.GetAllThemesAsync();

            var scheduledEvent = _mapper.Map<ScheduledEventEditViewModel>(await singleEventTask);
            scheduledEvent.AllStudentGroups = await studentGroupsTask;
            scheduledEvent.AllThemes = await themesTask;
            scheduledEvent.AllMentors = await mentorsTask;
            scheduledEvent.Color = singleEventTask.Result.Color;

            return scheduledEvent;
        }

        public async Task<EventOccurrenceEditViewModel> PrepareEventAddAsync()
        {
            var studentGroupsTask = _studentGroupService.GetAllStudentGroupsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var themesTask = _themeService.GetAllThemesAsync();

            var studentGroup = new EventOccurrenceEditViewModel
            {
                AllStudentGroups = await studentGroupsTask,
                AllThemes = await themesTask,
                AllMentors = await mentorsTask
            };

            return studentGroup;
        }

        public async Task UpdateSingleEventByIdAsync(long id, UpdateScheduledEventDto updatedSchedule)
        {
            var singleEvent =
               string.Format(_eventsApiEndpoints.UpdateEventEndpoint, id);

            await _apiUtil
                .PutAsync<EventOccurrenceDTO, UpdateScheduledEventDto>(singleEvent, updatedSchedule);
        }

        public async Task<EventOccurrenceEditViewModel> PrepareEventOcccurrenceUpdateAsync(long id)
        {
            var eventEndpoint =
               string.Format(_scheduleApiEndpoints.EventOccurrenceById, id);
            var eventDetailedEndpoint =
                string.Format(_scheduleApiEndpoints.EventOccurrenceDetailedById, id);

            var eventOccurrenceTask = _apiUtil.GetAsync<EventOccurrenceDTO>(eventEndpoint);
            var eventOccurrenceDetailedTask = _apiUtil.GetAsync<DetailedEventOccurrenceDTO>(eventDetailedEndpoint);
            var studentGroupsTask = _studentGroupService.GetAllStudentGroupsAsync();
            var mentorsTask = _mentorService.GetAllMentorsAsync();
            var themesTask = _themeService.GetAllThemesAsync();

            var scheduledEvent = _mapper.Map<EventOccurrenceEditViewModel>(await eventOccurrenceTask);
            scheduledEvent.AllStudentGroups = await studentGroupsTask;
            scheduledEvent.AllThemes = await themesTask;
            scheduledEvent.AllMentors = await mentorsTask;
            scheduledEvent.DetailedEventOccurrence = await eventOccurrenceDetailedTask;
            scheduledEvent.Color = eventOccurrenceTask.Result.Color;

            return scheduledEvent;
        }

        public async Task CreateSingleEventAsync(CreateSingleEventDto singleEventDTO)
        {
            await _apiUtil
              .CreateAsync<SingleEventDTO, CreateSingleEventDto>(_eventsApiEndpoints.AddSingleEvent, singleEventDTO);
        }

        public async Task DeleteSingleEventByIdAsync(long eventID)
        {
            var singleEvent = string.Format(_eventsApiEndpoints.DeleteEventById, eventID);

            await _apiUtil.DeleteAsync<bool>(singleEvent);
        }
    }
}
