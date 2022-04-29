using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.DTO.Event
{
    public class CreateSingleEventDTO
    {
        public long Id { get; set; }

        public long StudentGroupId { get; set; }

        public long? ThemeId { get; set; }

        public long? MentorId { get; set; }

        public long? LessonId { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
