using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class GetAllSchedulesResponse : IExamplesProvider<List<EventOccurrenceDTO>>
    {
        public List<EventOccurrenceDTO> GetExamples()
        {
            return new List<EventOccurrenceDTO>()
            {
                new EventOccurrenceDTO
                {
                    Id = 11,
                    StudentGroupId = 24,
                    EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                    EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                    Pattern = PatternType.Daily
                },
                new EventOccurrenceDTO
                {
                    Id = 12,
                    StudentGroupId = 31,
                    EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                    EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                    Pattern = PatternType.Weekly
                }
            };
        }
    }
}
