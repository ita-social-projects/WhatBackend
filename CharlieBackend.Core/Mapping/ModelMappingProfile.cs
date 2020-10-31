using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using System.Linq;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Student;

namespace CharlieBackend.Core.Mapping
{
    public class ModelMappingProfile: Profile
    {
        public ModelMappingProfile()
        {
            #region Accounts mapping

            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<Account, AuthenticationDto>();
            CreateMap<AuthenticationDto, Account>();

            #endregion


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


            #region Mentors mapping

            CreateMap<CreateMentorDto, Mentor>();
            CreateMap<Mentor, CreateMentorDto>();

            CreateMap<MentorDto, Mentor>();
            CreateMap<Mentor, MentorDto>();

            CreateMap<UpdateMentorDto, Mentor>();
            CreateMap<Mentor, UpdateMentorDto>();

            CreateMap<UpdateMentorDto, MentorDto>();
            CreateMap<MentorDto, UpdateMentorDto>();

            #endregion


            #region Students mapping
            CreateMap<CreateStudentDto, Student>();
            CreateMap<Student, CreateStudentDto>();

            CreateMap<StudentDto, Student>();
            CreateMap<Student, StudentDto>();

            CreateMap<UpdateStudentDto, Student>();
            CreateMap<Student, UpdateStudentDto>();

            CreateMap<UpdateStudentDto, StudentDto>();
            CreateMap<StudentDto, UpdateStudentDto>();
            #endregion


            #region StudentGroups mapping

            CreateMap<StudentGroup, StudentGroupDto>()
                  .ForMember(source => source.MentorIds, conf => conf.MapFrom(x => x.MentorsOfStudentGroups.Select(y => y.MentorId).ToList()))
                   .ForMember(source => source.StudentIds, conf => conf.MapFrom(x => x.StudentsOfStudentGroups.Select(y => y.StudentId).ToList()));

            CreateMap<UpdateStudentGroupDto, StudentGroup>();
            
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
