using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.SwaggerExamples.LessonsController
{
    internal class PostLessonRequest : IExamplesProvider<CreateLessonDto>
    {
        public CreateLessonDto GetExamples()
        {
            return new CreateLessonDto
            {
                ThemeName = "Some theme",
                MentorId = 2,
                LessonDate = new DateTime(2015, 7, 20, 18, 30, 25),
                StudentGroupId = 22,
                LessonVisits = new List<VisitDto>()
                {
                    new VisitDto()
                    {
                        StudentId = 44,
                        StudentMark = 5,
                        Presence = true,
                        Comment = ""
                    },
                    new VisitDto()
                    {
                        StudentId = 74,
                        Presence = false,
                        Comment = ""
                    },
                }
                
            };
        }
    }
}
