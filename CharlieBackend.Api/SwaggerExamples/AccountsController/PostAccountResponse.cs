using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;

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
                Role = UserRole.NotAssigned
            };
        }
    }
}
