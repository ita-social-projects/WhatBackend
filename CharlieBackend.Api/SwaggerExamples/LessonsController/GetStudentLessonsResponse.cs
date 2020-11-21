using CharlieBackend.Core.DTO.Lesson;
using CharlieBackend.Core.DTO.Visit;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Api.SwaggerExamples.LessonsController
{
    internal class GetStudentLessonsResponse : IExamplesProvider<IList<StudentLessonDto>>
    {
        public IList<StudentLessonDto> GetExamples()
        {
            return new List<StudentLessonDto>
            {
                new StudentLessonDto
                {
                    ThemeName = "Theme 2",
                    Id = 21,
                    Presence = true,
                    Mark = 5,
                    Comment = "Good responses",
                    StudentGroupId = 3,
                    LessonDate = new DateTime(2015, 8, 20, 18, 30, 25)
                },
                new StudentLessonDto
                {
                    ThemeName = "Theme",
                    Id = 22,
                    Presence = true,
                    Mark = 4,
                    Comment = "",
                    StudentGroupId = 3,
                    LessonDate = new DateTime(2015, 8, 21, 18, 30, 25)
                }
            };
        }
    }
}
