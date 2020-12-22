  using System.Collections.Generic;

namespace CharlieBackend.Core.Entities
{
    public partial class Theme : BaseEntity
    {
        public Theme()
        {
            Lessons = new HashSet<Lesson>();
        }

        public string Name { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }
    }
}
