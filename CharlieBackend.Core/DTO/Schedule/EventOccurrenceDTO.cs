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
        public long? Id { get; set; }

        public long StudentGroupId { get; set; }

        public DateTime EventStart { get; set; }
   
        public DateTime EventFinish { get; set; }

        public PatternType Pattern { get; set; }   

        public IList<ScheduledEventDTO> Events { get; set; }

        public long Storage { get; set; }
    }
}

