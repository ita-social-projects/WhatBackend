using CharlieBackend.Core.DTO.Schedule;

namespace CharlieBackend.Business.Services.ScheduleServiceFolder
{
    public class ScheduledEventHandlerDaily : ScheduledEventHandlerBase
    {
        public ScheduledEventHandlerDaily(PatternForCreateScheduleDTO pattern)
            : base(pattern)
        {
        }
    }
}
