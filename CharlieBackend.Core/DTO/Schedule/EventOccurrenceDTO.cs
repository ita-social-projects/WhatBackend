using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class EventOccurrenceDTO: EventOccurrence
    {
        public IList<ScheduledEventDTO> Events { get; set; }

    }
}

