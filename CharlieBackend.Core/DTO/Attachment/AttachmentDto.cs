using System;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        public long Id { get; set; }

        public DateTime AddTime { get; set; }

        public long UserId { get; set; }

        public UserRole UserRole { get; set; }

        [StringLength(100)]
        public string ContainerName { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }
    }
}
