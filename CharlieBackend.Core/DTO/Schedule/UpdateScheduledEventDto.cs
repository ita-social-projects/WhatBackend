using System;

namespace CharlieBackend.Core.DTO.Schedule
{
    public class UpdateScheduledEventDto
    {
        public long? StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public DateTime? EventStart { get; set; }

        public DateTime? EventEnd { get; set; }

        public int Color { get; set; }
    }

}

