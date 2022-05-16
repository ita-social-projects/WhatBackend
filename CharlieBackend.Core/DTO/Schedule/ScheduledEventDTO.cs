using CharlieBackend.Core.Entities;
using System;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class ScheduledEventDTO : ScheduledEvent
    {
        public long EventOccuranceId { get; set; }

    }
}
