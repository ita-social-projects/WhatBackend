using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class EventOccurrenceDTO
    {
        [Required]
        public long? Id { get; set; }

        [Required]
        public long StudentGroupId { get; set; }

        [Required]
        public DateTime EventStart { get; set; }

        [Required]        
        public DateTime EventFinish { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [EnumDataType(typeof(PatternType))]
        public PatternType Pattern { get; set; }   

        public IList<ScheduledEventDTO> Events { get; set; }
    }
}

