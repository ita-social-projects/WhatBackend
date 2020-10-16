namespace CharlieBackend.Core.Entities
{
    public partial class MentorOfStudentGroup : BaseEntity
    {
        public long? MentorId { get; set; }

        public long? StudentGroupId { get; set; }

        public virtual Mentor Mentor { get; set; }
        
        public virtual StudentGroup StudentGroup { get; set; }
    }
}
