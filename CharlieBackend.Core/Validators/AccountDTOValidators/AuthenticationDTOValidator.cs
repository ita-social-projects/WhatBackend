using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Core.Validators.AccountDTOValidators
{
    class AuthenticationDTOValidator : AbstractValidator<AuthenticationDto>
    {
        public AuthenticationDTOValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress().WithMessage("Incorrect email");
            RuleFor(x => x.Password)
                 .NotEmpty()
                 .Length(8, 50);
        }
    }
}
