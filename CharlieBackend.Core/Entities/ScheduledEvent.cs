﻿using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.Interfaces;

namespace CharlieBackend.Core.Entities
{
    public class ScheduledEvent : BaseEntity, ISoftDeletingModel
    {
        public long EventOccurrenceId { get; set; }

        public EventOccurrence EventOccurrence { get; set; }

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

        public bool IsDeleted { get; set; }
    }
}
