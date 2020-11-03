using AutoMapper;
using System.Linq;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.DTO.StudentGroups;

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

            #region Themes mapping


            CreateMap<ThemeDto, Theme>();
            CreateMap<Theme, ThemeDto>();

            CreateMap<CreateThemeDto, Theme>();
            CreateMap<Theme, CreateThemeDto>();

            #endregion

            #region Secretaries mapping

            CreateMap<SecretaryDto, Secretary>();
            CreateMap<Secretary, SecretaryDto>();

            CreateMap<CreateSecretaryDto, Secretary>();
            CreateMap<Secretary, CreateSecretaryDto>();

            #endregion

            #region StudentGroups mapping

            CreateMap<StudentGroup, StudentGroupDto>()
                  .ForMember(source => source.MentorIds, conf => conf.MapFrom(x => x.MentorsOfStudentGroups.Select(y => y.MentorId).ToList()))
                   .ForMember(source => source.StudentIds, conf => conf.MapFrom(x => x.StudentsOfStudentGroups.Select(y => y.StudentId).ToList()));

            CreateMap<UpdateStudentGroupDto, StudentGroup>();

            #endregion

        }
    }
}
