using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Core.Validators.AccountDTOValidators
{
    public class AuthenticationDTOValidator : AbstractValidator<AuthenticationDto>
    {
        public AuthenticationDTOValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(50).WithMessage("Email cannot be greateh than 50 symbols");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MaximumLength(65).WithMessage("Password cannot be greater than 65 symbols")
                .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Password must contains latin symbols and digits");
        }
    }
}
