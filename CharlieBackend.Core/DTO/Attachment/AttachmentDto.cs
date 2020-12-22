using System;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public long CreatedByAccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string ContainerName { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }
    }
}
