using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class UpdateScheduledEventDto
    {
        public long? StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        [DataType(DataType.Time)]
        public DateTime? EventStart { get; set; }

        [DataType(DataType.Time)]
        public DateTime? EventEnd { get; set; }
    }
}

