﻿using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.Entities;
using CharlieBackend.Panel.Models.Calendar;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    /// <summary>
    /// Class that provides methods for work with calendar and it's data.
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly IScheduleService _scheduleService;
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _getActiveCoursesEndpoint;
        private readonly string _getActiveMetorsEndpoint;
        private readonly string _getStudentGroupsEndpoint;
        private readonly string _getActiveStudentsEndpoint;
        private readonly string _getThemesEndpoint;


        private const int defaultDateFilterOffset = 7;

        public CalendarService(
            IScheduleService scheduleService,
            IApiUtil apiUtil,
            IMapper mapper,
            IOptions<ApplicationSettings> options,
            ICurrentUserService currentUserService)
        {
            _scheduleService = scheduleService;
            _apiUtil = apiUtil;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _getActiveCoursesEndpoint = options.Value.Urls.ApiEndpoints.Courses.GetAllCoursesEndpoint;
            _getActiveMetorsEndpoint = options.Value.Urls.ApiEndpoints.Mentors.ActiveMentorEndpoint;
            _getStudentGroupsEndpoint = options.Value.Urls.ApiEndpoints.StudentGroups.GetAllStudentGroupsEndpoint;
            _getActiveStudentsEndpoint = options.Value.Urls.ApiEndpoints.Students.ActiveStudentEndpoint;
            _getThemesEndpoint = options.Value.Urls.ApiEndpoints.Themes.GetAllThemesEndpoint;
        }

        /// <summary>
        /// Makes parallel asynchronous HTTP calls to API service and returns obtained data in a form
        /// of a ViewModel that is ready to be viewed in a calendar.
        /// </summary>
        /// <param name="scheduledEventFilter">DTO with an optional set of 
        /// filters for scheduled events.</param>
        /// <returns>>model with data required for viewing a calendar.</returns>
        public async Task<CalendarViewModel> GetCalendarDataAsync(
            ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            ApplyDefaultDateEventFilter(scheduledEventFilter);

            var getCoursesTask = GetActiveCourseViewModelsAsync();
            var getMentorsTask = GetActiveMetorViewModelsAsync();
            var getStudentGroupsTask = GetStudentGroupViewModelsAsync();
            IList<CalendarStudentViewModel> getStudentsTask = null;
            if (_currentUserService.Role == UserRole.Admin || _currentUserService.Role == UserRole.Secretary)
            {
                getStudentsTask = await GetActiveStudentViewModelsAsync();
            }
            else if (_currentUserService.Role == UserRole.Student)
            {
                getStudentsTask = new List<CalendarStudentViewModel>()
                {
                    new CalendarStudentViewModel()
                    {
                        Id = _currentUserService.EntityId,
                        Email = _currentUserService.Email,
                        FirstName = _currentUserService.FirstName,
                        LastName = _currentUserService.LastName
                    }
                };
            }
            var getThemesTask = GetThemeViewModelsAsync();
            var getEventOccurrencesTask = GetEventOccurrenceViewModelsAsync();
            var getScheduledEventsTask = GetScheduledEventViewModelsAsync(scheduledEventFilter);

            await Task.WhenAll(
                getCoursesTask, getMentorsTask, getStudentGroupsTask,
                    getThemesTask, getEventOccurrencesTask, getScheduledEventsTask
                );

            return new CalendarViewModel
            {
                Courses = getCoursesTask.Result,
                Mentors = getMentorsTask.Result,
                StudentGroups = getStudentGroupsTask.Result,
                Students = getStudentsTask,
                Themes = getThemesTask.Result,
                EventOccurences = getEventOccurrencesTask.Result,
                ScheduledEvents = getScheduledEventsTask.Result,
                ScheduledEventFilter = scheduledEventFilter
            };
        }

        /// <summary>
        /// Applies default DateTime filter for scheduled events
        /// in case it wasn't specified.
        /// </summary>
        /// <param name="scheduledEventFilter">Object with a set of 
        /// optional filtration parameters to add default values to.</param>
        private void ApplyDefaultDateEventFilter(
            ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            if (!scheduledEventFilter.StartDate.HasValue)
            {
                scheduledEventFilter.StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            }

            if (!scheduledEventFilter.FinishDate.HasValue)
            {
                scheduledEventFilter.FinishDate = scheduledEventFilter.StartDate.Value
                        .AddDays(defaultDateFilterOffset).AddMinutes(-1);
            }

            if (_currentUserService.Role == UserRole.Student)
            {
                scheduledEventFilter.StudentAccountID = _currentUserService.EntityId;
            }
        }

        private async Task<IList<CalendarCourseViewModel>> GetActiveCourseViewModelsAsync()
        {
            var activeCourseDtos = await _apiUtil.GetAsync<IList<CourseDto>>(_getActiveCoursesEndpoint);

            return _mapper.Map<IList<CalendarCourseViewModel>>(activeCourseDtos);
        }

        private async Task<IList<CalendarMentorViewModel>> GetActiveMetorViewModelsAsync()
        {
            var activeMetorDtos = await _apiUtil.GetAsync<IList<MentorDto>>(_getActiveMetorsEndpoint);

            return _mapper.Map<IList<CalendarMentorViewModel>>(activeMetorDtos);
        }

        private async Task<IList<CalendarStudentGroupViewModel>> GetStudentGroupViewModelsAsync()
        {
            var studentGroupDtos = await _apiUtil.GetAsync<IList<StudentGroupDto>>(_getStudentGroupsEndpoint);

            return _mapper.Map<IList<CalendarStudentGroupViewModel>>(studentGroupDtos);
        }

        private async Task<IList<CalendarStudentViewModel>> GetActiveStudentViewModelsAsync()
        {
            var activeStudentDtos = await _apiUtil.GetAsync<IList<StudentDto>>(_getActiveStudentsEndpoint);

            return _mapper.Map<IList<CalendarStudentViewModel>>(activeStudentDtos);
        }

        private async Task<IList<CalendarThemeViewModel>> GetThemeViewModelsAsync()
        {
            var themeDtos = await _apiUtil.GetAsync<IList<ThemeDto>>(_getThemesEndpoint);

            return _mapper.Map<IList<CalendarThemeViewModel>>(themeDtos);
        }

        private async Task<IList<CalendarEventOccurrenceViewModel>> GetEventOccurrenceViewModelsAsync()
        {
            var eventOccurrenceDtos = await _scheduleService.GetAllEventOccurrences();

            return _mapper.Map<IList<CalendarEventOccurrenceViewModel>>(eventOccurrenceDtos);
        }

        private async Task<IList<CalendarScheduledEventViewModel>> GetScheduledEventViewModelsAsync(
            ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            var scheduledEventDtos = await _scheduleService.GetEventsFiltered(scheduledEventFilter);

            return _mapper.Map<IList<CalendarScheduledEventViewModel>>(scheduledEventDtos);
        }
    }
}
