using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

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
                Visits = new List<VisitDto>()
                {
                    new VisitDto()
                    {
                        Id = 11,
                        StudentId = 45,
                        StudentMark = 5,
                        Presence = true,
                        Comment = ""
                    },
                    new VisitDto()
                    {
                        Id = 12,
                        StudentId = 74,
                        Presence = false,
                        Comment = ""
                    },
                }
            };
        }
    }
}
