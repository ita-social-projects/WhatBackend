namespace CharlieBackend.Core.Entities
{
    public partial class StudentOfStudentGroup : BaseEntity
    {
        public long? StudentGroupId { get; set; }
        public long? StudentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual StudentGroup StudentGroup { get; set; }
    }
}
