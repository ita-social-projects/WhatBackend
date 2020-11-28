using CharlieBackend.Core.DTO.Lesson;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.LessonsController
{
    public class AssignMentorToLessonRequest : IExamplesProvider<AssignMentorToLessonDto>
    {
        public AssignMentorToLessonDto GetExamples()
        {
            return new AssignMentorToLessonDto
            {
                LessonId = 5,
                MentorId = 6
            };
        }
    }
}
