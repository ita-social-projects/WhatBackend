using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Homework;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Panel.Models.Calendar;
using CharlieBackend.Panel.Models.Course;
using CharlieBackend.Panel.Models.Homework;
using CharlieBackend.Panel.Models.Lesson;
using CharlieBackend.Panel.Models.Mentor;
using CharlieBackend.Panel.Models.StudentGroups;
using CharlieBackend.Panel.Models.Students;
using System.Linq;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Models.ScheduledEvent;
using CharlieBackend.Panel.Models.EventOccurrence;

namespace CharlieBackend.Panel.Models.Mapping
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            CreateMap<StudentGroupDto, StudentGroupViewModel>()
                .ForMember(destination => destination.Students, config => config.MapFrom(x => x.StudentIds.Select(y => new StudentViewModel { Id = y }).ToList()))
                .ForMember(destination => destination.Mentors, config => config.MapFrom(x => x.MentorIds.Select(y => new MentorViewModel { Id = y }).ToList()))
                 .ForMember(destination => destination.Course, config => config.MapFrom(x => new CourseViewModel() { Id = x.CourseId}));

            CreateMap<StudentGroupDto, StudentGroupEditViewModel>()
               .ForMember(detination => detination.ActiveCourse, config => config.MapFrom(x => new CourseViewModel { Id = x.CourseId }))
               .ForMember(destination => destination.ActiveStudents, config => config.MapFrom(x => x.StudentIds.Select(y => new StudentViewModel { Id = y }).ToList()))
               .ForMember(destination => destination.ActiveMentors, config => config.MapFrom(x => x.MentorIds.Select(y => new MentorViewModel { Id = y }).ToList()));

            CreateMap<HomeworkViewModel, HomeworkDto>();
            CreateMap<HomeworkDto, HomeworkViewModel>();

            CreateMap<LessonViewModel, LessonDto>();
            CreateMap<LessonDto, LessonViewModel>()
                .ForMember(destination => destination.Mentor, config => config.MapFrom(x => new MentorViewModel() { Id = x.MentorId }))
                .ForMember(destination => destination.StudentGroup, config => config.MapFrom(x => new StudentGroupViewModel() { Id = (long)x.StudentGroupId}));

            CreateMap<StudentLessonDto, StudentLessonViewModel>();

            CreateMap<LessonDto, LessonVisitModel>()
               .ForMember(destination => destination.Visit, config => config.MapFrom(x => x.LessonVisits));

            CreateMap<LessonDto, LessonUpdateViewModel>();

            CreateMap<LessonUpdateViewModel, UpdateLessonDto>();

            CreateMap<LessonCreateViewModel, CreateLessonDto>()
                .ForMember(destination => destination.LessonVisits, config => config.MapFrom(x => x.LessonVisits))
                .ReverseMap();


            CreateMap<LessonCreateViewModel, LessonDto>();

            CreateMap<StudentGroupDto, UpdateStudentGroupDto>();

            CreateMap<StudentDto, StudentViewModel>();

            CreateMap<StudentViewModel, StudentViewModel>();

            CreateMap<MentorViewModel, MentorViewModel>();

            CreateMap<CourseDto, CourseViewModel>();

            #region Event ViewModelsMapping
            CreateMap<ScheduledEventViewModel, ScheduledEventDTO>();
            CreateMap<ScheduledEventDTO, ScheduledEventViewModel>();
            CreateMap<ScheduledEventDTO, ScheduledEventEditViewModel>();
            CreateMap<EventOccurrenceDTO, EventOccurrenceEditViewModel>();
            #endregion

            #region Calendar ViewModels mappings
            CreateMap<CourseDto, CalendarCourseViewModel>();
            CreateMap<MentorDto, CalendarMentorViewModel>();
            CreateMap<StudentGroupDto, CalendarStudentGroupViewModel>();
            CreateMap<StudentDto, CalendarStudentViewModel>();
            CreateMap<ThemeDto, CalendarThemeViewModel>();
            CreateMap<EventOccurrenceDTO, CalendarEventOccurrenceViewModel>();
            CreateMap<ScheduledEventDTO, CalendarScheduledEventViewModel>();
            #endregion
        }
    }
}
