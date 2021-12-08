using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Models.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class LessonVisitModel
    {
        public IList<StudentViewModel> Students { get; set; }

        public IList<VisitDto> Visit { get; set; }
    }
}
