using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using System.Linq;

namespace CharlieBackend.Core.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            #region Courses mapping

            CreateMap<CreateCourseDto, Course>();
            CreateMap<Course, CreateCourseDto>();

            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();

            CreateMap<UpdateCourseDto, Course>();
            CreateMap<Course, UpdateCourseDto>();

            CreateMap<UpdateCourseDto, CourseDto>();
            CreateMap<CourseDto, UpdateCourseDto>();

            #endregion

            #region Lessons mapping

            CreateMap<LessonDto, Lesson>();
            CreateMap<Lesson, LessonDto>();

            CreateMap<Lesson, CreateLessonDto>();


            CreateMap<CreateLessonDto, Lesson>()
                  .ForMember(destination => destination.Theme, conf => conf.MapFrom(x =>new Theme() { Name = x.ThemeName}))
                  .ForMember(destination => destination.Visits,
                             conf => conf.MapFrom(x => x.LessonVisits.Select(y => new Visit()
                             {
                                 StudentId = y.StudentId,
                                 StudentMark = y.StudentMark,
                                 Presence = y.Presence,
                                 Comment = y.Comment
                             }).ToList()));

            CreateMap<Lesson, LessonDto>()
                .ForMember(destination => destination.Visits, conf => conf.MapFrom(x => x.Visits.Select(y => new VisitDto()
                            {
                                 Id = y.Id,
                                 StudentId = y.StudentId,
                                 StudentMark = y.StudentMark,
                                 Presence = y.Presence,
                                 Comment = y.Comment
                            }).ToList()));


            CreateMap<Lesson, UpdateLessonDto>();
            CreateMap<UpdateLessonDto, Lesson>();

            #endregion

        }
    }
}
