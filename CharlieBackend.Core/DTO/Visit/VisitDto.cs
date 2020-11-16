using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Visit
{
    public class VisitDto
    {
        public long Id { get; set; }

        [Required]
        public long? StudentId { get; set; }

        public sbyte? StudentMark { get; set; }

        [Required]
        public bool Presence { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }
    }
}
