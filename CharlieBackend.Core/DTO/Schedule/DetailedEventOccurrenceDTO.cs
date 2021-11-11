namespace CharlieBackend.Core.DTO.Schedule
{
    public class DetailedEventOccurrenceDTO
    {
        public long? Id { get; set; }

        public PatternForCreateScheduleDTO Pattern { get; set; }

        public OccurenceRange Range { get; set; }

        public ContextForCreateScheduleDTO Context { get; set; }
    }
}
