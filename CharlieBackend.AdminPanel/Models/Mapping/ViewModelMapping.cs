using AutoMapper;
using CharlieBackend.AdminPanel.Models.Mentor;
using CharlieBackend.AdminPanel.Models.StudentGroups;
using CharlieBackend.AdminPanel.Models.Students;
using CharlieBackend.Core.DTO.StudentGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel.Models.Mapping
{
    public class ViewModelMapping: Profile
    {
        public ViewModelMapping()
        {
            #region Student groups mapping

            CreateMap<StudentGroupDto, StudentGroupViewModel>()
                .ForMember(destination => destination.Students, config => config.MapFrom(x => x.StudentIds.Select(y => new StudentViewModel { Id = y }).ToList()))
                .ForMember(destination => destination.Mentors, config => config.MapFrom(x => x.MentorIds.Select(y => new MentorViewModel { Id = y }).ToList()));

            CreateMap<StudentViewModel, StudentViewModel>();

            CreateMap<MentorViewModel, MentorViewModel>();

            #endregion
        }
    }
}
