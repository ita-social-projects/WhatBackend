using AutoMapper;
using CharlieBackend.Core.DTO.Course;
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
    public class ModelMappingProfile: Profile
    {
        public ModelMappingProfile()
        {
            #region Courses mapping

            CreateMap<CreateCourseDto, CreateCourseModel>();
            CreateMap<CreateCourseModel, CreateCourseDto>();

            CreateMap<CreateCourseModel, Course>();
            CreateMap<Course, CreateCourseModel>();

            CreateMap<CourseDto, CourseModel>();
            CreateMap<CourseModel, CourseDto>();

            CreateMap<UpdateCourseDto, UpdateCourseModel>();
            CreateMap<UpdateCourseModel, UpdateCourseDto>();

            CreateMap<Course, UpdateCourseModel>();
            CreateMap<UpdateCourseModel, Course>();

            CreateMap<CourseModel, Course>();
            CreateMap<Course, CourseModel>();

            #endregion

        }
    }
}
