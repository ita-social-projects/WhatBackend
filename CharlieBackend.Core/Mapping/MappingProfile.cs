using AutoMapper;
using CharlieBackend.Core.Entities;
using CharlieBackend.Core.Models;
using CharlieBackend.Core.Models.Course;
using CharlieBackend.Core.Models.Lesson;
using CharlieBackend.Core.Models.Mentor;
using CharlieBackend.Core.Models.Student;
using CharlieBackend.Core.Models.StudentGroup;
using CharlieBackend.Core.Models.Theme;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateThemeModel, Theme>(); // means you want to map from CreateThemeModel to Theme
            CreateMap<Theme, CreateThemeModel>();

            CreateMap<CreateStudentGroupModel, StudentGroup>()
                .ForMember("StartDate", x => x.MapFrom(y => Convert.ToDateTime(y.StartDate)))
                 .ForMember("FinishDate", x => x.MapFrom(y => Convert.ToDateTime(y.FinishDate)));

            CreateMap<CreateStudentModel, Account>();
            CreateMap<Student, StudentModel>();

            CreateMap<CreateMentorModel, Account>();
            CreateMap<Mentor, MentorModel>();

            CreateMap<Lesson, LessonModel>();

            CreateMap<BaseAccountModel, Account>();
            CreateMap<Account, BaseAccountModel>();

            CreateMap<CourseModel, Course>();
            CreateMap<Course, CourseModel>();

        }
    }
}
