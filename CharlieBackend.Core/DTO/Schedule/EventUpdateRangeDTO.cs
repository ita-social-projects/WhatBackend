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
        [Required]
        public ScheduledEventFilterRequestDTO Filter { get; set; }

        [Required]
        public UpdateScheduledEventDto Request { get; set; }

    }
}
