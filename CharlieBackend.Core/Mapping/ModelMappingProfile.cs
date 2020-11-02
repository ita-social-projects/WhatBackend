using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.Entities;

namespace CharlieBackend.Core.Mapping
{
    public class ModelMappingProfile: Profile
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

            CreateMap<Lesson, CreateLessonDto>()
                .ForMember(destination => destination.StudentGroupId, source => source.MapFrom(x => x.Visits));
            CreateMap<CreateLessonDto, Lesson>();

            CreateMap<Lesson, UpdateLessonDto>();
            CreateMap<UpdateLessonDto, Lesson>();
            #endregion

        }
    }
}
