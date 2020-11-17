using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.DTO.Theme
{
    public class UpdateThemeDto
    {
        [Required]
        public string Name { get; set; }
    }
}
