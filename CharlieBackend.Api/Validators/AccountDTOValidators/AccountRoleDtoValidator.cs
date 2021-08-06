using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class AccountRoleDtoValidator : AbstractValidator<AccountRoleDto>
    {
        public AccountRoleDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);              
        }
    }
}
