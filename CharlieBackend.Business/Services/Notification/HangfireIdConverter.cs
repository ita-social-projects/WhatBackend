using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Business.Services.Notification
{
    public static class HangfireIdConverter
    {
        public static string CreateCustomHomeworkJobId(long homeworkId)
        {
            return $"homework{homeworkId}";
        }

        public static string CreateCustomScheduledEventJobId(long scheduledEventId)
        {
            return $"scheduledevent{scheduledEventId}";
        }
    }
}
