using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.LessonsController
{
    public class AssignMentorToLessonResponse : IExamplesProvider<Lesson>
    {
        public Lesson GetExamples()
        {
            return new Lesson()
            {
                Id = 5,
                LessonDate = new DateTime(2015, 7, 20, 18, 30, 25),
                MentorId = 6,
                StudentGroupId = 21,
                ThemeId = 131,
                Visits = new List<Visit>
                {
                    new Visit
                    {
                        Id = 11,
                        StudentId = 45,
                        StudentMark = 5,
                        Presence = true,
                        Comment = "",
                        LessonId = 42
                    }
                }
            };
        }
    }
}
