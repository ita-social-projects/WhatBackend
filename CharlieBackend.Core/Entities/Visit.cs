namespace CharlieBackend.Core.Entities
{
    public partial class Visit : BaseEntity
    {
        public long StudentId { get; set; }

        public long? LessonId { get; set; }

        public long? MarkId { get; set; }

        public bool Presence { get; set; }

        public virtual Lesson Lesson { get; set; }
        
        public virtual Student Student { get; set; }

        public virtual Mark Mark { get; set; }
    }
}
