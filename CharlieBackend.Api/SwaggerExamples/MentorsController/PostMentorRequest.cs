using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.MentorsController
{
    internal class PostMentorRequest : IExamplesProvider<MentorDto>
    {
        public MentorDto GetExamples()
        {
            return new MentorDto
            {
                Id = 44,
                Email = "mentor@example.com",
                FirstName = "Steve",
                LastName = "Vozniac"
            };
        }
    }
}
