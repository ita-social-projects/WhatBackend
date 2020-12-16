using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string ContainerName { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }
    }
}
