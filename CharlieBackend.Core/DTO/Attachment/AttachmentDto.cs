using System;
using System.Text;
using System.Collections.Generic;


namespace CharlieBackend.Core.DTO.Attachment
{
    public class AttachmentDto
    {
        public long Id { get; set; }

        public string ContainerName { get; set; }

        public string FileName { get; set; }
    }
}
