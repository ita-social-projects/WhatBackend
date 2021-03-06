using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Core.Entities;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class AccountDtoValidator : AbstractValidator<AccountDto>
    {
        public AccountDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.FirstName)
                 .NotEmpty()
                 .MaximumLength(30);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(30);
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(50);
            RuleFor(x => x.Role)
                .NotEmpty()
                .IsInEnum();
            RuleFor(x => x.IsActive)
                .NotEmpty();
        }
    }
}
