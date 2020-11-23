using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class PutScheduleRequest : IExamplesProvider<UpdateScheduleDto>
    {
        public UpdateScheduleDto GetExamples()
        {
            return new UpdateScheduleDto()
            {
                LessonStart = new TimeSpan(09, 15, 00),
                LessonEnd = new TimeSpan(10, 00, 00),
                DayNumber = 3,
                RepeatRate = RepeatRate.Daily
            };
        }
    }
}
