using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Theme
{
    public class CreateThemeDto
    {
        [Required]
        public string Name { get; set; }
    }
}
