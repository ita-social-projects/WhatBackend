using CharlieBackend.Core.DTO.Dashboard;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Classbook
{
    public class FilteredClassbookViewModel
    {       
        public IList<StudentMarkDto> StudentsMarks { get; set; }

        public IList<StudentVisitDto> StudentsPresences { get; set; }        
    }
}
