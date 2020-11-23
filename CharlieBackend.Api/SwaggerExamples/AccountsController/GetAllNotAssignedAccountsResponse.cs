using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.AccountsController
{
    internal class GetAllNotAssignedAccountsResponse : IExamplesProvider<List<AccountDto>>
    {
        public List<AccountDto> GetExamples()
        {
            return new List<AccountDto>
            {
                new AccountDto()
                    {
                        Id = 55,
                        Email = "example3@example.com",
                        FirstName = "James",
                        LastName = "Bond",
                        IsActive = true,
                        Role = UserRole.NotAssigned
                    },
                    new AccountDto()
                    {
                        Id = 56,
                        Email = "example4@example.com",
                        FirstName = "Danny",
                        LastName = "Horsch",
                        IsActive = true,
                        Role = UserRole.NotAssigned
                    }
            };
        }
    }
}
