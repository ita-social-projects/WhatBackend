using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CharlieBackend.Core.DTO.Visit
{
    public class VisitDto
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [JsonPropertyName("student_id")]
        public long? StudentId { get; set; }

        [JsonPropertyName("student_mark")]
        public sbyte? StudentMark { get; set; }

        [Required]
        [JsonPropertyName("presence")]
        public bool Presence { get; set; }

        [JsonPropertyName("comment")]
        [StringLength(1024)]
        public string Comment { get; set; }
    }
}
