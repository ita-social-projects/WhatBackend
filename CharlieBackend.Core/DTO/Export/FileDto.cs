using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Export
{
    public class FileDto
    {
        public byte[] ByteArray { get; set; }

        public string Filename { get; set; }

        public string ContentType { get; set; }
    }
}
