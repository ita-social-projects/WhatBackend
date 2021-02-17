using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class PostScheduleResponse : IExamplesProvider<EventOccurrenceDTO>
    {
        public EventOccurrenceDTO GetExamples()
        {
            return new EventOccurrenceDTO()
            {
                Id = 12,
                StudentGroupId = 31,
                EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                Pattern = PatternType.Weekly
            };
        }
    }
}
