using AutoMapper;
using System.Linq;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.DTO.Mentor;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.DTO.Theme;
using CharlieBackend.Core.DTO.Secretary;
using CharlieBackend.Core.DTO.StudentGroups;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.DTO.Course;

namespace CharlieBackend.Core.Mapping
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            #region Accounts mapping

            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<Account, AuthenticationDto>();
            CreateMap<AuthenticationDto, Account>();

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
                .ForMember(destination => destination.LessonVisits, conf => conf.MapFrom(x => x.Visits.Select(y => new VisitDto()
                            {
                                 StudentId = y.StudentId,
                                 StudentMark = y.StudentMark,
                                 Presence = y.Presence,
                                 Comment = y.Comment
                            }).ToList()));


            CreateMap<Lesson, UpdateLessonDto>();
            CreateMap<UpdateLessonDto, Lesson>();

            CreateMap<LessonDto, UpdateLessonDto>();
            CreateMap<UpdateLessonDto, LessonDto>();
            #endregion


            #region Mentors mapping

            CreateMap<MentorDto, Mentor>();
            CreateMap<Mentor, MentorDto>()
                .ForMember(source => source.Email, conf => conf.MapFrom(x => x.Account.Email))
                .ForMember(source => source.FirstName, conf => conf.MapFrom(x => x.Account.FirstName))
                .ForMember(source => source.LastName, conf => conf.MapFrom(x => x.Account.LastName));

            CreateMap<UpdateMentorDto, Mentor>();
            CreateMap<Mentor, UpdateMentorDto>();

            CreateMap<UpdateMentorDto, MentorDto>();
            CreateMap<MentorDto, UpdateMentorDto>();

            #endregion


            #region Students mapping

            CreateMap<StudentDto, Student>();
            CreateMap<Student, StudentDto>()
                .ForMember(source => source.Email, conf => conf.MapFrom(x => x.Account.Email))
                .ForMember(source => source.FirstName, conf => conf.MapFrom(x => x.Account.FirstName))
                .ForMember(source => source.LastName, conf => conf.MapFrom(x => x.Account.LastName));

            CreateMap<UpdateStudentDto, Student>();
            CreateMap<Student, UpdateStudentDto>();

            CreateMap<UpdateStudentDto, StudentDto>();
            CreateMap<StudentDto, UpdateStudentDto>();
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


            #region Themes mapping

            CreateMap<ThemeDto, Theme>();
            CreateMap<Theme, ThemeDto>();

            CreateMap<CreateThemeDto, Theme>();
            CreateMap<Theme, CreateThemeDto>();
            CreateMap<UpdateThemeDto, Theme>();

            #endregion


            #region Secretaries mapping

            CreateMap<SecretaryDto, Secretary>();
            CreateMap<Secretary, SecretaryDto>()
                .ForMember(source => source.Email, conf => conf.MapFrom(x => x.Account.Email))
                .ForMember(source => source.FirstName, conf => conf.MapFrom(x => x.Account.FirstName))
                .ForMember(source => source.LastName, conf => conf.MapFrom(x => x.Account.LastName));

            CreateMap<UpdateSecretaryDto, Secretary>();
            CreateMap<Secretary, UpdateSecretaryDto>();

            CreateMap<UpdateSecretaryDto, SecretaryDto>();
            CreateMap<SecretaryDto, UpdateSecretaryDto>();

            #endregion


            #region Schedules mapping

            CreateMap<CreateScheduleDto, Schedule>();
            CreateMap<UpdateScheduleDto, Schedule>();
            CreateMap<Schedule, ScheduleDto>();

            #endregion

        }
    }
}
