namespace CharlieBackend.Core.Entities
{
    public partial class Visit : BaseEntity
    {
        public long? StudentId { get; set; }

        public long? LessonId { get; set; }

        public sbyte? StudentMark { get; set; }

        public bool Presence { get; set; }

        public string Comment { get; set; }

        public virtual Lesson Lesson { get; set; }
        
        public virtual Student Student { get; set; }
    }
}
