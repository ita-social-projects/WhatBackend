using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using CharlieBackend.Core.DTO.Secretary;

namespace CharlieBackend.Api.SwaggerExamples.SecretariesController
{
    internal class PostSecretaryRequest : IExamplesProvider<SecretaryDto>
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
