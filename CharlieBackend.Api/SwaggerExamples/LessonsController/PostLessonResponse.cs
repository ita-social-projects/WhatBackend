using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.LessonsController
{
    internal class PostLessonResponse : IExamplesProvider<LessonDto>
    {
        public LessonDto GetExamples()
        {
            return new LessonDto()
            {
                Id = 34,
                ThemeName = "Some theme",
                MentorId = 2,
                LessonDate = new DateTime(2015, 7, 20, 18, 30, 25),
                StudentGroupId = 22,
                LessonVisits = new List<VisitDto>()
                {
                    new VisitDto()
                    {
                        StudentId = 45,
                        StudentMark = 5,
                        Presence = true
                    },
                    new VisitDto()
                    {
                        StudentId = 74,
                        Presence = false,
                        StudentMark = null
                    },
                }
            };
        }
    }
}
