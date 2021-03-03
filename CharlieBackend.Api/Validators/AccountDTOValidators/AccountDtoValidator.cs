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
                .NotEmpty().WithMessage("{PropertyName} is required")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("{PropertyName} is required")
                 .MaximumLength(30).WithMessage("{PropertyName} can't be greater than {MaxLength} symbols");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} can't be greater than {MaxLength} symbols");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(50).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .IsInEnum().WithMessage("Invalid role");
            RuleFor(x => x.IsActive)
                .NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
