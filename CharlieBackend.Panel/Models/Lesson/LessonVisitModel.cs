using CharlieBackend.Core.DTO.Visit;
using CharlieBackend.Panel.Models.Students;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.Lesson
{
    public class LessonVisitModel
    {
        public IList<StudentViewModel> Students { get; set; }

        public IList<VisitDto> Visit { get; set; }
    }
}
