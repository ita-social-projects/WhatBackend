using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required]
        public long CreatedByAccountId { get; set; }

        [Required]
        public string ContainerName { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
