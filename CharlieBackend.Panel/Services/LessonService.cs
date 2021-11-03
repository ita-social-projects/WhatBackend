using AutoMapper;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.ScheduledEvent;
using CharlieBackend.Panel.Models.Students;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class LessonService : ILessonService
    {
        private readonly LessonsApiEndpoints _lessonsApiEndpoints;
        private readonly IMentorService _mentorService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IThemeService _themeService;
        private readonly IStudentService _studentService;
        private readonly IScheduleService _scheduleService;
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        public LessonService(IOptions<ApplicationSettings> options, 
            IApiUtil apiUtil, 
            IMapper mapper, 
            IMentorService mentorService,
            IStudentGroupService studentGroupService,
            IThemeService themeService,
            IStudentService studentService,
            IScheduleService scheduleService)
        {
            _lessonsApiEndpoints = options.Value.Urls.ApiEndpoints.Lessons;
            _apiUtil = apiUtil;
            _mapper = mapper;
            _mentorService = mentorService;
            _studentGroupService = studentGroupService;
            _themeService = themeService;
            _studentService = studentService;
            _scheduleService = scheduleService;
        }

        public async Task AddLessonEndpoint(LessonCreateViewModel createLesson)
        {
            var createLessonDto = _mapper.Map<CreateLessonDto>(createLesson);
            var addLessonEndpoint = _lessonsApiEndpoints.AddLessonEndpoint;
            await _apiUtil.CreateAsync(addLessonEndpoint, createLessonDto);
        }

        public async Task<LessonCreateViewModel> PrepareLessonAddAsync()
        {
            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync();
            var mentors = _mentorService.GetAllMentorsAsync();
            var themes = _themeService.GetAllThemesAsync();
            var students = studentGroups.Where(x => x.Id == 5).FirstOrDefault().Students;
            //var students = await _studentService.GetAllStudentsAsync();

            var newLesson = new LessonCreateViewModel()
            {
                StudentGroups = studentGroups,
                Mentors = await mentors,
                Themes = await themes,
                Students = students
                
            };

            return newLesson;
        }

        //public async Task<IList<StudentViewModel>> StudentsPartial(long id)
        //{
        //    var groups = await _studentGroupService.GetAllStudentGroupsAsync();
        //    var students = groups.Where(x => x.Id == id).FirstOrDefault().Students;

        //    return students;
        //}

        public async Task<LessonViewModel> GetLessonById(long id)
        {
            var getLessonById = string.Format(_lessonsApiEndpoints.GetLessonById, id);
            var lessonDto = await _apiUtil.GetAsync<LessonDto>(getLessonById);
            return _mapper.Map<LessonViewModel>(lessonDto);
        }

        public async Task<IList<LessonViewModel>> GetLessonsByDate()
        {
            var lessonsEndpoint = _lessonsApiEndpoints.GetLessonsByDate;
            var allLessonsResponse = await _apiUtil.GetAsync<IList<LessonDto>>(lessonsEndpoint);
            var allLessons = _mapper.Map<IList<LessonViewModel>>(allLessonsResponse);

            var mentors = await _mentorService.GetAllMentorsAsync();
            var stGroups = await _studentGroupService.GetAllStudentGroupsAsync();

            foreach (var item in allLessons)
            {
                item.Mentor.FirstName = mentors.Where(x => x.Id == item.Mentor.Id).FirstOrDefault()?.FirstName;
                item.Mentor.LastName = mentors.Where(x => x.Id == item.Mentor.Id).FirstOrDefault()?.LastName;
                item.StudentGroup.Name = stGroups.Where(x => x.Id == item.StudentGroup.Id).FirstOrDefault()?.Name;
            }

            return allLessons;
           
        }

        public async Task UpdateLessonEndpoint(long id, LessonDto lessonDto)
        {
            var updateLessonEndpoint = string.Format(_lessonsApiEndpoints.UpdateLessonEndpoint, id);
            await _apiUtil.PutAsync(updateLessonEndpoint, lessonDto);
        }

        public async Task<LessonVisitModel> LessonVisits(long id)
        {
            var getLessonById = string.Format(_lessonsApiEndpoints.GetLessonById, id);
            var lessonDto = await _apiUtil.GetAsync<LessonDto>(getLessonById);
            
            var attendance = _mapper.Map<LessonVisitModel>(lessonDto);

            //var students = await _studentService.GetAllStudentsAsync();

            //foreach (var item in attendance.Students)
            //{
            //    item.FirstName = students.Where(x => x.Id == item.Id).FirstOrDefault().FirstName;
            //}

            return attendance;
        }

        public async Task<IList<ScheduledEventViewModel>> Get2MounthEvents()
        {
            //var eve = awa
            var evDt = new ScheduledEventFilterRequestDTO
            {
                StartDate = DateTime.Now.AddDays(1),
                FinishDate = DateTime.Now.AddDays(2)
            };
            var events = await _scheduleService.GetEventsFiltered(evDt);
            var mapped = _mapper.Map<IList<ScheduledEventViewModel>>(events);

            return mapped;
        }

        public async Task<IList<LessonViewModel>> GetLessonsForMentorAsync(FilterLessonsRequestDto filterModel)
        {
            if (filterModel is null)
            {
                throw new ArgumentNullException();
            }

            var result = await _apiUtil.PostAsync<IList<LessonViewModel>, FilterLessonsRequestDto>(
                url: _lessonsApiEndpoints.GetLessonsForMentor,
                data: filterModel);

            return result;
        }
    }
}
