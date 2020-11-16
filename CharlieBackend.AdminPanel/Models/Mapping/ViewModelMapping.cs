using AutoMapper;
using CharlieBackend.AdminPanel.Models.Course;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.Student;
using CharlieBackend.Core.DTO.StudentGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Models.Mapping
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            #region Student groups mapping

            CreateMap<StudentGroupDto, StudentGroupViewModel>()
                .ForMember(destination => destination.Students, config => config.MapFrom(x => x.StudentIds.Select(y => new StudentViewModel { Id = y }).ToList()))
                .ForMember(destination => destination.Mentors, config => config.MapFrom(x => x.MentorIds.Select(y => new MentorViewModel { Id = y }).ToList()));


            CreateMap<StudentGroupDto, StudentGroupEditViewModel>()
               .ForMember(detination => detination.ActiveCourse, config => config.MapFrom(x => new CourseViewModel { Id = x.CourseId }))
               .ForMember(destination => destination.ActiveStudents, config => config.MapFrom(x => x.StudentIds.Select(y => new StudentViewModel { Id = y }).ToList()))
               .ForMember(destination => destination.ActiveMentors, config => config.MapFrom(x => x.MentorIds.Select(y => new MentorViewModel { Id = y }).ToList()));

            CreateMap<StudentGroupDto, UpdateStudentGroupDto>();

            CreateMap<StudentGroupDto, UpdateStudentsForStudentGroup>();

            #endregion


            #region Student mapping

            CreateMap<StudentDto, StudentViewModel>();

            CreateMap<StudentViewModel, StudentViewModel>();

            #endregion


            #region Mentors mapping

            CreateMap<MentorViewModel, MentorViewModel>();

            #endregion


            #region Courses mapping

            CreateMap<CourseDto, CourseViewModel>();

            #endregion

        }
    }
}
