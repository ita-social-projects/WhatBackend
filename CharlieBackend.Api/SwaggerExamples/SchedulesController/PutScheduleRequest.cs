using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.SchedulesController
{
    internal class PutScheduleRequest : IExamplesProvider<UpdateScheduledEventDto>
    {
        public UpdateScheduledEventDto GetExamples()
        {
            return new UpdateScheduledEventDto()
            {
                EventStart = DateTime.Now,
                EventEnd = DateTime.Now.AddHours(1),
                MentorId = 0,
                StudentGroupId = 0,
                ThemeId = 0
            };
        }
    }
}
