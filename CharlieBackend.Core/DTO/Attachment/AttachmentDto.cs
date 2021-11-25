using System;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedByAccountId { get; set; }

        public string ContainerName { get; set; }

        public string FileName { get; set; }
    }
}
