using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Dashboard;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Panel.Models.Classbook;
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
    public class ClassbookService : IClassbookService
    {
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly string _getActiveCoursesEndpoint;
        private readonly string _getStudentGroupsEndpoint;
        private readonly string _getStudentsClassbookEndpoint;

        public ClassbookService(
            IApiUtil apiUtil,
            IMapper mapper,
            IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;
            _mapper = mapper;
            _getActiveCoursesEndpoint = options.Value.Urls.ApiEndpoints.Courses.GetAllCoursesEndpoint;
            _getStudentGroupsEndpoint = options.Value.Urls.ApiEndpoints.StudentGroups.GetAllStudentGroupsEndpoint;
            _getStudentsClassbookEndpoint = options.Value.Urls.ApiEndpoints.Dashboard.GetStudentsClassbookEndpoint;
        }

        /// <summary>
        /// Makes parallel asynchronous HTTP calls to API service and returns obtained data in a form
        /// of a ViewModel that is ready to be viewed in a classbook.
        /// </summary>
        /// <param name="classbookFilter">DTO with an optional set of 
        /// filters for classbook.</param>
        /// <returns>>model with data required for viewing a classbook.</returns>
        public async Task<ClassbookViewModel> GetClassbookAsync(
            StudentsRequestDto<ClassbookResultType> classbookFilter)
        {
            ApplyDefaultClassbookFilter(classbookFilter);

            var getCoursesTask = GetActiveCourseViewModelsAsync();
            var getStudentGroupsTask = GetStudentsGroupViewModelsAsync();
            var getFilteredRegisterTask = GetFilteredRegisterViewModelAsynk(classbookFilter);

            await Task.WhenAll(getCoursesTask, getStudentGroupsTask, getFilteredRegisterTask);

            return new ClassbookViewModel
            {
                Courses = getCoursesTask.Result,
                StudentGroups = getStudentGroupsTask.Result,
                FilteredClassbook = getFilteredRegisterTask.Result,
                ClassbookFilter = classbookFilter
            };
        }

        /// <summary>
        /// Applies default classbook filter.
        /// </summary>
        /// <param name="classbookFilter">Object with a set of 
        /// optional filtration parameters to add default values to.</param>
        private void ApplyDefaultClassbookFilter(
            StudentsRequestDto<ClassbookResultType> classbookFilter)
        {
            if (classbookFilter.StartDate==default(DateTime))
            {
                classbookFilter.StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (classbookFilter.FinishDate==default(DateTime))
            {
                classbookFilter.FinishDate = DateTime.Now;
            }
            if (classbookFilter.CourseId==null)
            {
                classbookFilter.CourseId = 1;
            }
            if (classbookFilter.StudentGroupId==null)
            {
                classbookFilter.StudentGroupId = 1;
            }
            if (classbookFilter.IncludeAnalytics==null)
            {
                classbookFilter.IncludeAnalytics = new ClassbookResultType[] {
                ClassbookResultType.StudentPresence, ClassbookResultType.StudentMarks };
            }
        }

        private async Task<IList<ClassbookCourseViewModel>> GetActiveCourseViewModelsAsync()
        {
            var activeCourseDtos = await _apiUtil.GetAsync<IList<CourseDto>>(_getActiveCoursesEndpoint);

            return _mapper.Map<IList<ClassbookCourseViewModel>>(activeCourseDtos);
        }

        private async Task<IList<ClassbookStudentsGroupViewModel>> GetStudentsGroupViewModelsAsync()
        {
            var studentGroupDtos = 
                await _apiUtil.GetAsync<IList<StudentGroupDto>>(_getStudentGroupsEndpoint);

            return _mapper.Map<IList<ClassbookStudentsGroupViewModel>>(studentGroupDtos);
        }
        
        private async Task<FilteredClassbookViewModel> GetFilteredRegisterViewModelAsynk(
            StudentsRequestDto<ClassbookResultType> groupsRegisterFilter)
        {
            var result = await _apiUtil.PostAsync< StudentsClassbookResultDto, StudentsRequestDto <ClassbookResultType>>(
                url: _getStudentsClassbookEndpoint,
                data: groupsRegisterFilter);

            return _mapper.Map<FilteredClassbookViewModel>(result);
        }
    }
}
