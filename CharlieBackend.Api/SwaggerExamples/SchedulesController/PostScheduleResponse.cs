using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class PostScheduleResponse : IExamplesProvider<EventOccurenceDTO>
    {
        public EventOccurenceDTO GetExamples()
        {
            return new EventOccurenceDTO()
            {
                Id = 12,
                StudentGroupId = 31,
                EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                DayNumber = 2,
                RepeatRate = PatternType.Weekly
            };
        }
    }
}
