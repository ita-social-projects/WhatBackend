using CharlieBackend.Core.DTO.Schedule;
using System;

namespace CharlieBackend.Core.Entities
{
    public class ScheduledEvent : BaseEntity
    {
        public long? EventOccurrenceId { get; set; }

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

        public int Color { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UpdateScheduledEventDto)
            {
                var item = (obj as UpdateScheduledEventDto);
                return MentorId.Equals(item.MentorId)
                    && StudentGroupId.Equals(item.StudentGroupId)
                    && ThemeId.Equals(item.ThemeId)
                    && EventStart.Equals(item.EventStart)
                    && EventFinish.Equals(item.EventEnd);
            }
            else if(obj is ScheduledEvent)
            {
                var item = (obj as ScheduledEvent);
                return Id == item.Id;
                   
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
