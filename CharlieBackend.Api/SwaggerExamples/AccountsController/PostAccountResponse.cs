using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.AccountsController
{
    internal class PostAccountResponse : IExamplesProvider<AccountDto>
    {
        public AccountDto GetExamples()
        {
            return new AccountDto()
            {
                Id = 45,
                Email = "example@example.com",
                FirstName = "Bob",
                LastName = "Marley",
                IsActive = true,
                Role = UserRole.Student
            };
        }
    }
}
