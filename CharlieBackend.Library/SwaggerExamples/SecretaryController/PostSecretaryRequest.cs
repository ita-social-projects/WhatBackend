using CharlieBackend.Core.DTO.Secretary;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.SecretaryController
{
    public class PostSecretaryRequest : IExamplesProvider<SecretaryDto>
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
