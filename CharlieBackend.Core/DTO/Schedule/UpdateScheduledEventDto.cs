using System;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class UpdateScheduledEventDto: ScheduledEventDTO
    {

        public DateTime? EventEnd { get; set; }
    }
}

