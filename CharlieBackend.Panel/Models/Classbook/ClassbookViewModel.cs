using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Classbook
{
    public class ClassbookViewModel
    {
        public IList<ClassbookCourseViewModel> Courses { get; set; }

        public IList<ClassbookStudentsGroupViewModel> StudentGroups { get; set; }

        public FilteredClassbookViewModel FilteredClassbook { get; set; }

        public StudentsRequestDto<ClassbookResultType> ClassbookFilter { get; set; }
    }
}
