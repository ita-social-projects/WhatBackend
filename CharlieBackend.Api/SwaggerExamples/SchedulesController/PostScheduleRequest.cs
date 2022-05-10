using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class PostScheduleRequest : IExamplesProvider<CreateScheduleDto>
    {
        public CreateScheduleDto GetExamples()
        {
            return new CreateScheduleDto
            {
                Pattern = new PatternForCreateScheduleDTO
                {
                    Type = PatternType.RelativeMonthly,

                    Interval = 2,

                    DaysOfWeek = new List<DayOfWeek>
                    {
                        DayOfWeek.Monday,
                        DayOfWeek.Friday
                    },

                    Index = MonthIndex.Second,
                },

                Range = new OccurenceRange
                {
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now
                },

                Context = new ContextForCreateScheduleDTO
                {
                    GroupID = 1,
                    MentorID = 5,
                    ThemeID = 6,
                    ColorID = 2
                }
            };
        }
    }
}
