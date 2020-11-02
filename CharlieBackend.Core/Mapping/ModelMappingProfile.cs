﻿using AutoMapper;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Secretary;
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
