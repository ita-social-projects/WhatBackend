using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class GetAllSchedulesResponse : IExamplesProvider<List<EventOccurenceDTO>>
    {
        public List<EventOccurenceDTO> GetExamples()
        {
            return new List<EventOccurenceDTO>()
            {
                new EventOccurenceDTO
                {
                    Id = 11,
                    StudentGroupId = 24,
                    EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                    EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                    DayNumber = 3,
                    RepeatRate = PatternType.Daily
                },
                new EventOccurenceDTO
                {
                    Id = 12,
                    StudentGroupId = 31,
                    EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                    EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                    DayNumber = 2,
                    RepeatRate = PatternType.Weekly
                }
            };
        }
    }
}
