using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class PostScheduleRequest : IExamplesProvider<CreateScheduleDto>
    {
        public CreateScheduleDto GetExamples()
        {
            return new CreateScheduleDto
            {
                StudentGroupId = 123,
                LessonStart = new TimeSpan(9, 15, 00),
                LessonEnd = new TimeSpan(10, 00, 00),
                DayNumber = 2,
                RepeatRate = RepeatRate.Weekly
            };
        }
    }
}
