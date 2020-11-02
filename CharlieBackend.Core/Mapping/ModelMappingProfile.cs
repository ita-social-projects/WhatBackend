using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;

using CharlieBackend.Core.Entities;
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

            CreateMap<StudentGroup, StudentGroupDto>()
                  .ForMember(source => source.MentorIds, 
                             conf => conf.MapFrom(x => x.MentorsOfStudentGroups.
                                          Select(y => y.MentorId).ToList()))
                   .ForMember(source => source.StudentIds,
                              conf => conf.MapFrom(x => x.StudentsOfStudentGroups.
                                          Select(y => y.StudentId).ToList()));

            CreateMap<UpdateStudentGroupDto, StudentGroup>();
            CreateMap<StudentGroup, UpdateStudentGroupDto>();

            CreateMap<UpdateStudentsForStudentGroup, StudentGroup>()
               .ForMember(source => source.StudentsOfStudentGroups, 
                          conf => conf.MapFrom(x => x.StudentIds.
                                       Select(x => new StudentOfStudentGroup() 
                                       { 
                                           StudentId = x 

                                       }).ToList()));

            CreateMap<StudentGroup, UpdateStudentsForStudentGroup>()
              .ForMember(source => source.StudentIds, 
                        conf => conf.MapFrom(x => x.StudentsOfStudentGroups.
                                     Select(y => y.StudentId).ToList()));

            #endregion


            #region Theme mapping

            CreateMap<ThemeDto, Theme>();
            CreateMap<Theme, ThemeDto>();

            CreateMap<CreateThemeDto, Theme>();
            CreateMap<Theme, CreateThemeDto>();

            #endregion

        }
    }
}
