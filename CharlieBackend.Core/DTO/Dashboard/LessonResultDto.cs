using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Dashboard
{
    public class LessonResultDto
    {
        public long? LessonId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LessonDate { get; set; }

        public bool Presence { get; set; }

        public sbyte? StudentMark { get; set; }

        [StringLength(1024)]
        public string Comment { get; set; }
    }

}
