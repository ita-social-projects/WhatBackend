using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.File
{
    public class ImportFileDto
    {
        [Required]
        public string url { get; set; }
    }
}
