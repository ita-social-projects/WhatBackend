using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Core.Validators.AccountDTOValidators
{
    public class AuthenticationDtoValidator : AbstractValidator<AuthenticationDto>
    {
        public AuthenticationDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(50).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MaximumLength(65).WithMessage("Password cannot be greater than {MaxLength} symbols");
        }
    }
}
