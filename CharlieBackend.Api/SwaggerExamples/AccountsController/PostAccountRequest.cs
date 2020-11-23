using CharlieBackend.Core.DTO.Account;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.AccountsController
{
    internal class PostAccountRequest : IExamplesProvider<CreateAccountDto>
    {
        public CreateAccountDto GetExamples()
        {
            return new CreateAccountDto()
            {
                Email = "example@example.com",
                FirstName = "Bob",
                LastName = "Marley",
                Password = "qwerty1!",
                ConfirmPassword = "qwerty1!"
            };
        }
    }
}
