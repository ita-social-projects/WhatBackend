using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CharlieBackend.Core.Models
{
    public class ThemeModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
