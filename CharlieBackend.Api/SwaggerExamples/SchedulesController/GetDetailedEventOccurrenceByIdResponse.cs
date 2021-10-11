using CharlieBackend.Core.DTO.Schedule;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    /// <summary>
    /// Example for DetailedEventOccurrenceDTO
    /// </summary>
    public class GetDetailedEventOccurrenceByIdResponse : IExamplesProvider<DetailedEventOccurrenceDTO>
    {
        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <returns>Example</returns>
        public DetailedEventOccurrenceDTO GetExamples()
        {
            return new DetailedEventOccurrenceDTO
            {
                Id = 5,
                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = 5,
                    MentorID = null,
                    ThemeID = null
                },
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = 0,
                    Interval = 1,
                    DaysOfWeek = new DayOfWeek[1],
                    Index = MonthIndex.Second,
                    Dates = null
                },
                Range = new OccurenceRange
                {
                    StartDate = DateTime.Parse("2021-01-04T10:00:00"),
                    FinishDate = DateTime.Parse("2021-12-24T11:00:00")
                }
            };
        }
    }
}
