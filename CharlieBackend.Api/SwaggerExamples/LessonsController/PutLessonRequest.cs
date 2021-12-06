using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.LessonsController
{
    internal class PutLessonRequestExample : IExamplesProvider<UpdateLessonDto>
    {
        public UpdateLessonDto GetExamples()
        {
            return new UpdateLessonDto
            {
                LessonDate = new DateTime(2015, 8, 20, 18, 30, 25),
                ThemeName = "New theme name",
                LessonVisits = new List<VisitDto>
                 {
                    new VisitDto
                    {
                          StudentId = 43,
                          StudentMark = 5,
                          Presence = true,
                          Comment = ""
                    },
                    new VisitDto
                    {
                          StudentId = 45,
                          StudentMark = 4,
                          Presence = true,
                          Comment = ""
                    }
                 }
            };
        }
    }
}
