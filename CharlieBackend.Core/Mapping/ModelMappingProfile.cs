using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models.StudentGroup;
using System.Linq;

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

            #region StudentGroups mapping

            //CreateMap<StudentGroupDto, StudentGroup>() FIX
            //    .ForMember(source => source.MentorsOfStudentGroups, conf => conf.MapFrom(x => x.MentorIds.Select(y => new MentorOfStudentGroup() { MentorId = y} ).ToList()))
            //     .ForMember(source => source.StudentsOfStudentGroups, conf => conf.MapFrom(x => x.StudentIds.Select(y => new StudentOfStudentGroup() { StudentId = y }).ToList()));

            CreateMap<StudentGroup, StudentGroupDto>()
                  .ForMember(source => source.MentorIds, conf => conf.MapFrom(x => x.MentorsOfStudentGroups.Select(y => y.MentorId).ToList()))
                   .ForMember(source => source.StudentIds, conf => conf.MapFrom(x => x.StudentsOfStudentGroups.Select(y => y.StudentId).ToList()));

            #endregion

        }
    }
}
