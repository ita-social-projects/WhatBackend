using CharlieBackend.Core.DTO.Account;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.AccountsController
{
    public class SignInRequest : IExamplesProvider<AuthenticationDto>
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
