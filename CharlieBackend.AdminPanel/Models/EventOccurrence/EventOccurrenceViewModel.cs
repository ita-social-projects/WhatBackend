using CharlieBackend.Core.DTO.Schedule;
using System;

namespace CharlieBackend.AdminPanel.Models.EventOccurrence
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
    }
}
