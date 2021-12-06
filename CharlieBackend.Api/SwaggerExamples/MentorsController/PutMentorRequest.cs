using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.MentorsController
{
    internal class PutMentorRequest : IExamplesProvider<UpdateMentorDto>
    {
        public UpdateMentorDto GetExamples()
        {
            return new UpdateMentorDto
            {
                Email = "mentor@example.com",
                FirstName = "Steve",
                LastName = "Vozniac",
                CourseIds = new List<long>
                { 12, 52 },
                StudentGroupIds = new List<long>
                { 15, 19 }
            };
        }
    }
}
