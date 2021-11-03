using System;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentRequestDto
    {
        public long? MentorID;

        public long? CourseID;

        public long? GroupID;

        public long? StudentAccountID;

        public DateTime? StartDate;

        public DateTime? FinishDate;
    }
}
