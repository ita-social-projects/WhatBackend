using CharlieBackend.Core.DTO.Schedule;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    /// <summary>
    /// 
    /// </summary>
    public class GetFilteredRequest : IExamplesProvider<ScheduledEventFilterRequestDTO>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ScheduledEventFilterRequestDTO GetExamples()
        {
            return new ScheduledEventFilterRequestDTO()
            {
                ThemeID = 0,
                CourseID = 0,
                MentorID = 0,
                StudentAccountID = 0,
                GroupID = 0,
                EventOccurrenceID = 0,
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddHours(1)
            };
        }
    }
}
