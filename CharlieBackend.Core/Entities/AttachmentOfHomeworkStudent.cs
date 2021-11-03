namespace CharlieBackend.Core.Entities
{
    public partial class AttachmentOfHomeworkStudent : BaseEntity
    {
        public long HomeworkStudentId { get; set; }

        public long AttachmentId { get; set; }

        public HomeworkStudent HomeworkStudent { get; set; }

        public Attachment Attachment { get; set; }
    }
}
