using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentRequestDto
    {
        public long? CourseID;

        public long? GroupID;

        public long? StudentID;

        [DataType(DataType.DateTime)]
        public DateTime? StartDate;

        [DataType(DataType.DateTime)]
        public DateTime? FinishDate;
    }
}
