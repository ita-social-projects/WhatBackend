using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Models.ScheduledEvent
{
    public class ScheduledEventViewModel
    {
        public long Id { get; set; }

        //public long EventOccuranceId { get; set; }

        public long StudentGroupId { get; set; }
        public string StudentGroupName { get; set; }

        public long? ThemeId { get; set; }
        

        public long? MentorId { get; set; }

        public long? LessonId { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
