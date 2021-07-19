using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CharlieBackend.Core.DTO.Schedule
{
    public class EventUpdateRangeDTO
    {
        public ScheduledEventFilterRequestDTO Filter { get; set; }

        public UpdateScheduledEventDto Request { get; set; }

    }
}
