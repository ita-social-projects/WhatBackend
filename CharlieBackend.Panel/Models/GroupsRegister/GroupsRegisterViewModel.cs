using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.GroupsRegister
{
    public class GroupsRegisterViewModel
    {
        public IList<GroupsRegisterCourseViewModel> Courses { get; set; }

        public IList<GroupsRegisterStudentsGroupViewModel> StudentGroups { get; set; }

        public IList<FilteredRegisterViewModel> FilteredRegister { get; set; }

        public StudentsRequestDto<Enum> GroupsRegisterFilter { get; set; }
    }
}
