using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.MentorsController
{
    internal class PutMentorResponse : IExamplesProvider<MentorDto>
    {
        public MentorDto GetExamples()
        {
            return new MentorDto
            {
                Id = 47,
                Email = "mentor@example.com",
                FirstName = "Steve",
                LastName = "Donaldson"
            };
        }
    }
}
