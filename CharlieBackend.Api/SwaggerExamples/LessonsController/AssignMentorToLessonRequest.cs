using CharlieBackend.Core.DTO.Lesson;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.LessonsController
{
    internal class AssignMentorToLessonRequest : IExamplesProvider<AssignMentorToLessonDto>
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
