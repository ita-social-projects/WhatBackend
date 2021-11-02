using CharlieBackend.Core.DTO.Secretary;
using Swashbuckle.AspNetCore.Filters;

namespace CharlieBackend.Api.SwaggerExamples.SecretariesController
{
    class PutSecretaryRequest : IExamplesProvider<UpdateSecretaryDto>
    {
        public UpdateSecretaryDto GetExamples()
        {
            return new UpdateSecretaryDto
            {
                Email = "newemailofsecretary@example.com",
                FirstName = "Olivia",
                LastName = "Sanders"
            };
        }
    }
}
