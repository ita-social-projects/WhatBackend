using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Library.SwaggerExamples.AccountsController
{
    public class GetAllAccountResponse : IExamplesProvider<IList<AccountDto>>
    {
        public IList<AccountDto> GetExamples()
        {
            return new List<AccountDto>
            {
                new AccountDto()
                    {
                        Id = 45,
                        Email = "example@example.com",
                        FirstName = "Bob",
                        LastName = "Marley",
                        IsActive = true,
                        Role = UserRole.Student
                    },
                    new AccountDto()
                    {
                        Id = 49,
                        Email = "example2@example.com",
                        FirstName = "Elvise",
                        LastName = "Presley",
                        IsActive = true,
                        Role = UserRole.Secretary
                    }
            };
        }
    }
}
