using CharlieBackend.Core.DTO.Secretary;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.SecretariesController
{
    class PostSecretaryRequest : IExamplesProvider<SecretaryDto>
    {
        public SecretaryDto GetExamples()
        {
            return new SecretaryDto()
            {
                Id = 145,
                Email = "secretaryemail@example.com",
                FirstName = "Isabella",
                LastName = "Smith",
            };
        }
    }
}
