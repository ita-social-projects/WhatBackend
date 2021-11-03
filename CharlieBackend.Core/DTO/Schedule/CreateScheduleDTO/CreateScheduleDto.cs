namespace CharlieBackend.Core.DTO.Schedule
{
    public class CreateScheduleDto
    {
        public PatternForCreateScheduleDTO Pattern { get; set; }

        public OccurenceRange Range { get; set; }

        public ContextForCreateScheduleDTO Context { get; set; }
    }
}

