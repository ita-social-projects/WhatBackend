using CharlieBackend.Core.DTO;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Panel.Models.EventOccurrence
{
    public class EventOccurrenceViewModel
    {
        public static  EventOccurrenceViewModel FromEventOccurrenceDTO(
                EventOccurrenceDTO eventOccurrenceDTO)
        {
            return new EventOccurrenceViewModel
            {
                Id = eventOccurrenceDTO.Id,
                Storage = eventOccurrenceDTO.Storage,
                StudentGroupId = eventOccurrenceDTO.StudentGroupId,
                EventStart = eventOccurrenceDTO.EventStart,
                EventFinish = eventOccurrenceDTO.EventFinish
            };
        }

        public long? Id { get; set; }

        public long StudentGroupId { get; set; }

        public DateTime? EventStart { get; set; }

        public DateTime? EventFinish { get; set; }

        public long Storage { get; set; }

        public IList<EventColorDTO> EventColors { get; set; }
    }
}
