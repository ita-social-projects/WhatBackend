﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Entities
{
    public class ScheduledEvent : BaseEntity
    {
        public long EventOccurenceId { get; set; }

        public EventOccurrence EventOccurence { get; set; }

        public long? StudentGroupId { get; set; }

        public StudentGroup StudentGroup { get; set; }

        public long? ThemeId { get; set; }

        public Theme Theme { get; set; }

        public long? MentorId { get; set; }

        public Mentor Mentor { get; set; }

        public long? LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventFinish { get; set; }
    }
}
