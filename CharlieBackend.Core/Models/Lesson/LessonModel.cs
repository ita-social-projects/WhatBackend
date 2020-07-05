using CharlieBackend.Core.Models.Visit;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.Models.Lesson
{
    public class LessonModel
    {
        public virtual long Id { get; set; }
        public virtual string ThemeName { get; set; }

        [DataType(DataType.Date)]
        public virtual string LessonDate { get; set; }
        public virtual long GroupId { get; set; }
        public virtual VisitModel[] LessonVisits { get; set; }
    }
}
