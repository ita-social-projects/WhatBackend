using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Panel.Models.GroupsRegister;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    /// <summary>
    /// Class that provides methods for work with group's registers.
    /// </summary>
    public class GroupsRegisterService : IGroupsRegisterService
    {
        private readonly IScheduleService _scheduleService;
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly string _getActiveCoursesEndpoint;
        private readonly string _getStudentGroupsEndpoint;
        private readonly string _getThemesEndpoint;

        private const int defaultDateFilterOffset = -7;

        public GroupsRegisterService(
            IScheduleService scheduleService,
            IApiUtil apiUtil,
            IMapper mapper,
            IOptions<ApplicationSettings> options)
        {
            _scheduleService = scheduleService;
            _apiUtil = apiUtil;
            _mapper = mapper;
            _getActiveCoursesEndpoint = options.Value.Urls.ApiEndpoints.Courses.GetAllCoursesEndpoint;
            _getStudentGroupsEndpoint = options.Value.Urls.ApiEndpoints.StudentGroups.GetAllStudentGroupsEndpoint;
            _getThemesEndpoint = options.Value.Urls.ApiEndpoints.Themes.GetAllThemesEndpoint;
        }

        /// <summary>
        /// Makes parallel asynchronous HTTP calls to API service and returns obtained data in a form
        /// of a ViewModel that is ready to be viewed in a group's register.
        /// </summary>
        /// <param name="registerFilter">DTO with an optional set of 
        /// filters for group's register.</param>
        /// <returns>>model with data required for viewing a register.</returns>
        public async Task<FilteredRegisterViewModel> GetRegisterAsync(
            StudentsRequestDto<Enum> groupsRegisterFilter)
        {
            ApplyDefaultDateRegisterFilter(groupsRegisterFilter);

            var getCoursesTask = GetActiveCourseViewModelsAsync();
            var getStudentGroupsTask = GetStudentsGroupViewModelsAsync();
            var getFilteredRegisterTask = GetFilteredRegisterViewModelAsynk();
            var getGroupsRegistorTask = GetGroupsRegisterModelsAsync(groupsRegisterFilter);

            await Task.WhenAll(getCoursesTask, getStudentGroupsTask, getFilteredRegisterTask, getGroupsRegistorTask);

            return new FilteredRegisterViewModel
            {
                Courses = getCoursesTask.Result,
                StudentGroups = getStudentGroupsTask.Result,
                FilteredRegister = getFilteredRegisterTask.Result,
                GroupsRegisterFilter = groupsRegisterFilter
            };
        }

        /// <summary>
        /// Applies default DateTime filter for scheduled events
        /// in case it wasn't specified.
        /// </summary>
        /// <param name="scheduledEventFilter">Object with a set of 
        /// optional filtration parameters to add default values to.</param>
        private void ApplyDefaultDateRegisterFilter(
            StudentsRequestDto<Enum> groupsRegisterFilter)
        {
            //if (!groupsRegisterFilter.StartDate.HasValue)
            //{
            groupsRegisterFilter.StartDate = DateTime.Now;
            //}

            //if (!scheduledEventFilter.FinishDate.HasValue)
            //{
            groupsRegisterFilter.FinishDate = DateTime.Now.AddDays(defaultDateFilterOffset);
            //}
        }

        private async Task<IList<GroupsRegisterCourseViewModel>> GetActiveCourseViewModelsAsync()
        {
            var activeCourseDtos = await _apiUtil.GetAsync<IList<CourseDto>>(_getActiveCoursesEndpoint);

            return _mapper.Map<IList<GroupsRegisterCourseViewModel>>(activeCourseDtos);
        }

        private async Task<IList<GroupsRegisterStudentsGroupViewModel>> GetStudentsGroupViewModelsAsync()
        {
            var studentGroupDtos = await _apiUtil.GetAsync<IList<StudentGroupDto>>(_getStudentGroupsEndpoint);

            return _mapper.Map<IList<GroupsRegisterStudentsGroupViewModel>>(studentGroupDtos);
        }

        private async Task<IList<FilteredRegisterViewModel>> GetFilteredRegisterViewModelAsynk(
            StudentsRequestDto<Enum> groupsRegisterFilter)
        {
            var filteredRegisterDtos = await _scheduleService.GetEventsFiltered(groupsRegisterFilter);

            return _mapper.Map<IList<CalendarScheduledEventViewModel>>(scheduledEventDtos);
        }
    }
}
