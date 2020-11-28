using CharlieBackend.Core.DTO.Account;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.AccountsController
{
    internal class SignInRequest : IExamplesProvider<AuthenticationDto>
    {
        public AuthenticationDto GetExamples()
        {
            return new AuthenticationDto()
            {
                Email = "email@example.com",
                Password = "qwerty"
            };
        }
    }
}
