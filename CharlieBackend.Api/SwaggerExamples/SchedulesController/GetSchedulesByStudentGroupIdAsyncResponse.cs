using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class GetSchedulesByStudentGroupIdAsyncResponse : IExamplesProvider<IList<EventOccurrenceDTO>>
    {
        public IList<EventOccurrenceDTO> GetExamples()
        {
            return new List<EventOccurrenceDTO>()
            {
                new EventOccurrenceDTO
                {
                    Id = 14,
                    StudentGroupId = 24,
                    EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                    EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                    Pattern = PatternType.Daily
                },
                new EventOccurrenceDTO
                {
                    Id = 15,
                    StudentGroupId = 24,
                    EventStart = new DateTime(2020, 10, 12, 10, 15, 00),
                    EventFinish = new DateTime(2020, 10, 12, 10, 15, 00),
                    Pattern = PatternType.Weekly
                }
            };
        }
    }
}
