using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using CharlieBackend.Core.DTO.Account;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(50).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.NewPassword)
               .NotEmpty().WithMessage("{PropertyName} is required")
               .MaximumLength(30).WithMessage("Password cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.NewPassword).WithMessage("Passwords does not match");
        }
    }
}
