using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Mentor : BaseEntity
    {
        public Mentor()
        {
            Lesson = new HashSet<Lesson>();
            MentorsOfCourses = new HashSet<MentorOfCourse>();
            MentorsOfStudentGroups = new HashSet<MentorOfStudentGroup>();
        }

        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }

        public virtual ICollection<Lesson> Lesson { get; set; }

        public virtual ICollection<MentorOfCourse> MentorsOfCourses { get; set; }

        public virtual ICollection<MentorOfStudentGroup> MentorsOfStudentGroups { get; set; }

        public virtual ICollection<ScheduledEvent> ScheduledEvents { get; set; }
    }
}
