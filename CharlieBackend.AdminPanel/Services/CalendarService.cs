using AutoMapper;
using CharlieBackend.AdminPanel.Models.Calendar;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Services
{
    /// <summary>
    /// Class that provides methods for work with calendar and it's data.
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly IScheduleService _scheduleService;
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;

        private IList<CourseDto> _activeCourseDtos;
        private IList<MentorDto> _activeMetorDtos;
        private IList<StudentGroupDto> _studentGroupDtos;
        private IList<StudentDto> _activeStudentDtos;
        private IList<ThemeDto> _themeDtos;
        private IList<EventOccurrenceDTO> _eventOccurrenceDtos;
        private IList<ScheduledEventDTO> _scheduledEventDtos;
        private ScheduledEventFilterRequestDTO _scheduledEventFilter;

        private const int defaultDateFilterOffset = 15;

        public CalendarService(
            IScheduleService scheduleService,
            IApiUtil apiUtil,
            IMapper mapper)
        {
            _scheduleService = scheduleService;
            _apiUtil = apiUtil;
            _mapper = mapper;
        }

        public async Task<CalendarViewModel> GetCalendarDataAsync(
            ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            ApplyEventFilter(scheduledEventFilter);

            await GetDataFromApiAsync();

            var calendarViewModel = InitCalendarViewModel();

            return calendarViewModel;
        }

        /// <summary>
        /// Applies default DateTime filter for scheduled events
        /// in case it wasn't specified.
        /// </summary>
        private void ApplyEventFilter(
            ScheduledEventFilterRequestDTO scheduledEventFilter)
        {
            if (!scheduledEventFilter.StartDate.HasValue)
            {
                scheduledEventFilter.StartDate = DateTime.Now.AddDays(-defaultDateFilterOffset);
            }

            if (!scheduledEventFilter.FinishDate.HasValue)
            {
                scheduledEventFilter.FinishDate = DateTime.Now.AddDays(defaultDateFilterOffset);
            }

            _scheduledEventFilter = scheduledEventFilter;
        }

        /// <summary>
        /// Makes HTTP calls to Web API and obtains required data.
        /// </summary>
        private async Task GetDataFromApiAsync()
        {
            _activeCourseDtos = await _apiUtil.GetAsync<IList<CourseDto>>("api/courses/isActive");

            _activeMetorDtos = await _apiUtil.GetAsync<IList<MentorDto>>("api/mentors/active");

            _studentGroupDtos = await _apiUtil.GetAsync<IList<StudentGroupDto>>("api/student_groups");

            _activeStudentDtos = await _apiUtil.GetAsync<IList<StudentDto>>("api/students/active");

            _themeDtos = await _apiUtil.GetAsync<IList<ThemeDto>>("api/themes");

            _eventOccurrenceDtos = await _scheduleService.GetAllEventOccurrences();

            _scheduledEventDtos = await _scheduleService.GetEventsFiltered(_scheduledEventFilter);
        }

        /// <summary>
        /// Initializes and returns new <see cref="CalendarViewModel"/> instance
        /// with data obtained from Web API service.
        /// </summary>
        private CalendarViewModel InitCalendarViewModel()
        {
            return new CalendarViewModel
            {
                Courses = _mapper.Map<IList<CalendarCourseViewModel>>(_activeCourseDtos),
                Mentors = _mapper.Map<IList<CalendarMentorViewModel>>(_activeMetorDtos),
                StudentGroups = _mapper.Map<IList<CalendarStudentGroupViewModel>>(_studentGroupDtos),
                Students = _mapper.Map<IList<CalendarStudentViewModel>>(_activeStudentDtos),
                Themes = _mapper.Map<IList<CalendarThemeViewModel>>(_themeDtos),
                EventOccurences = _mapper.Map<IList<CalendarEventOccurrenceViewModel>>(_eventOccurrenceDtos),
                ScheduledEvents = _mapper.Map<IList<CalendarScheduledEventViewModel>>(_scheduledEventDtos),
                ScheduledEventFilter = _scheduledEventFilter
            };
        }
    }
}
