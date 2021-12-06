using CharlieBackend.Core.DTO.Mentor;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.StudentsController
{
    class GetMentorByIdResponse : IExamplesProvider<MentorDto>
    {

        public MentorDto GetExamples()
        {
            return new MentorDto
            {
                FirstName = "Brad",
                LastName = "Pitt",
                Email = "student@example.com"
            };
        }
    }

}
