using AutoMapper;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
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
        private readonly IEventsService _eventsService;
        private readonly IApiUtil _apiUtil;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public LessonService(IOptions<ApplicationSettings> options, 
            IApiUtil apiUtil, 
            IMapper mapper, 
            IMentorService mentorService,
            IStudentGroupService studentGroupService,
            IThemeService themeService,
            IStudentService studentService,
            IScheduleService scheduleService,
            IEventsService eventsService,
            ICurrentUserService currentUserService)
        {
            _lessonsApiEndpoints = options.Value.Urls.ApiEndpoints.Lessons;
            _apiUtil = apiUtil;
            _mapper = mapper;
            _mentorService = mentorService;
            _studentGroupService = studentGroupService;
            _themeService = themeService;
            _studentService = studentService;
            _scheduleService = scheduleService;
            _eventsService = eventsService;
            _currentUserService = currentUserService;
        }

        public async Task AddLessonEndpoint(LessonCreateViewModel createLesson)
        {
            var createLessonDto = _mapper.Map<CreateLessonDto>(createLesson);
            var addLessonEndpoint = _lessonsApiEndpoints.AddLessonEndpoint;
            await _apiUtil.CreateAsync(addLessonEndpoint, createLessonDto);
            if (createLesson.EventId != null)
            {
                var lessonDto = _mapper.Map<LessonDto>(createLesson);
                lessonDto.Id = GetLessonsByDate().GetAwaiter().GetResult().Last().Id;
                await _eventsService.ConnectScheduleToLessonById((long)createLesson.EventId, lessonDto);
            }
        }

        public async Task<LessonCreateViewModel> PrepareLessonAddAsync(long stGroupId)
        {
            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync();
            var mentors = _mentorService.GetAllActiveMentorsAsync();
            var themes = _themeService.GetAllThemesAsync();
            var students = studentGroups.Where(x => x.Id == stGroupId).FirstOrDefault().Students;

            var newLesson = new LessonCreateViewModel()
            {
                StudentGroups = studentGroups,
                StudentGroupId = studentGroups.Where(x => x.Id == stGroupId).Select(i => i.Id).FirstOrDefault(),
                Mentors = await mentors,
                Themes = await themes,
                Students = students,
                LessonVisits = students.Select(s => new VisitDto { StudentId = s.Id }).ToList()
            };

            return newLesson;
        }

        public async Task<LessonDto> GetLessonById(long id)
        {
            var getLessonById = string.Format(_lessonsApiEndpoints.GetLessonById, id);
            var lessonDto = await _apiUtil.GetAsync<LessonDto>(getLessonById);
            return lessonDto;
        }

        public async Task<IList<LessonViewModel>> GetLessonsByDate()
        {
            var lessonsEndpoint = _lessonsApiEndpoints.GetLessonsByDate;
            var allLessonsResponse = await _apiUtil.GetAsync<IList<LessonDto>>(lessonsEndpoint);
            var allLessons = _mapper.Map<IList<LessonViewModel>>(allLessonsResponse);

            var mentors = await _mentorService.GetAllActiveMentorsAsync();
            var stGroups = await _studentGroupService.GetAllStudentGroupsAsync();

            foreach (var item in allLessons)
            {
                item.Mentor.FirstName = mentors.Where(x => x.Id == item.Mentor.Id).FirstOrDefault()?.FirstName;
                item.Mentor.LastName = mentors.Where(x => x.Id == item.Mentor.Id).FirstOrDefault()?.LastName;
                item.StudentGroup.Name = stGroups.Where(x => x.Id == item.StudentGroup.Id).FirstOrDefault()?.Name;
            }

            return allLessons;
           
        }

        public async Task UpdateLessonEndpoint(long id, LessonUpdateViewModel lessonToUpdate)
        {
            var updateLessonDto = _mapper.Map<UpdateLessonDto>(lessonToUpdate);
            var updateLessonEndpoint = string.Format(_lessonsApiEndpoints.UpdateLessonEndpoint, id);
            await _apiUtil.PutAsync(updateLessonEndpoint, updateLessonDto);
        }

        public async Task<LessonVisitModel> LessonVisits(long id)
        {
            var getLessonById = string.Format(_lessonsApiEndpoints.GetLessonById, id);
            var lessonDto = await _apiUtil.GetAsync<LessonDto>(getLessonById);
            
            var attendance = _mapper.Map<LessonVisitModel>(lessonDto);

            var sudentIds = attendance.Visit.Select(x => x.StudentId).ToArray();
            var students = await _studentService.GetAllStudentsAsync();

            var filteredSt = new List<StudentViewModel>();

            foreach (var item in students)
            {
                for (int i = 0; i < sudentIds.Length; i++)
                {
                    if (item.Id == sudentIds[i])
                    {
                        filteredSt.Add(item);
                    }
                }
            }

            attendance.Students = filteredSt;

            return attendance;
        }

        public async Task<IList<ScheduledEventViewModel>> Get2DaysEvents()
        {
            const int numberOfDays = 2;

            var evDt = new ScheduledEventFilterRequestDTO
            {
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddDays(numberOfDays)        
            };

            if (_currentUserService.Role == UserRole.Mentor)
            {
                evDt.MentorID = _currentUserService.EntityId;
            }

            var events = await _scheduleService.GetEventsFiltered(evDt);
            var evWithoutLesson = events.Where(x => x.LessonId == null);
            var mapped = _mapper.Map<IList<ScheduledEventViewModel>>(evWithoutLesson);

            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync();
            var themes = await _themeService.GetAllThemesAsync();
            var mentors = await _mentorService.GetAllActiveMentorsAsync();

            foreach (var item in mapped)
            {
                item.ThemeName = themes.Where(x => x.Id == item.ThemeId).FirstOrDefault().Name;
                item.StudentGroupName = studentGroups.Where(x => x.Id == item.StudentGroupId).FirstOrDefault().Name;
                item.MentorFirstName = mentors.Where(x => x.Id == item.MentorId).FirstOrDefault().FirstName;
                item.MentorLastName = mentors.Where(x => x.Id == item.MentorId).FirstOrDefault().LastName;
            }

            return mapped;
        }

        public async Task<IList<LessonViewModel>> GetAllLessonsForMentor(long mentorId)
        {
            var mentorLessons = string.Format(_lessonsApiEndpoints.GetAllLessonsForMentor, mentorId);
            var lessonsDto = await _apiUtil.GetAsync<IList<LessonDto>>(mentorLessons);
            var lessons = _mapper.Map<IList<LessonViewModel>>(lessonsDto);

            var mentor = await _mentorService.GetMentorByIdAsync(mentorId);
            var stGroups = await _studentGroupService.GetAllStudentGroupsAsync();

            foreach (var lesson in lessons)
            {
                lesson.Mentor.FirstName = mentor.FirstName;
                lesson.Mentor.LastName = mentor.LastName;
                lesson.StudentGroup.Name = stGroups.Where(x => x.Id == lesson.StudentGroup.Id).FirstOrDefault()?.Name;
            }

            return lessons;
        }

        public async Task<IList<StudentLessonViewModel>> GetStudentLessonsAsync(long studentId)
        {
            var studentLesson = string.Format(_lessonsApiEndpoints.GetStudentLessonsAsync, studentId);
            var stLessonDto = await _apiUtil.GetAsync<IList<StudentLessonDto>>(studentLesson);
            var lessons = _mapper.Map<IList<StudentLessonViewModel>>(stLessonDto);

            return lessons;
        }

        public async Task<LessonUpdateViewModel> PrepareLessonUpdateAsync(long id)
        {
            var lessonDto = await GetLessonById(id);
            var studentGroups = await _studentGroupService.GetAllStudentGroupsAsync();

            var lessonToUpdate = _mapper.Map<LessonUpdateViewModel>(lessonDto);
            lessonToUpdate.Themes = await _themeService.GetAllThemesAsync();
            lessonToUpdate.Students = studentGroups.Where(x => x.Id == lessonDto.StudentGroupId).FirstOrDefault().Students;

            return lessonToUpdate;
        }

    }
}
