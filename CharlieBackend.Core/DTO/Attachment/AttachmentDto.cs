using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        public long Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedByAccountId { get; set; }

        [StringLength(100)]
        public string ContainerName { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }
    }
}
