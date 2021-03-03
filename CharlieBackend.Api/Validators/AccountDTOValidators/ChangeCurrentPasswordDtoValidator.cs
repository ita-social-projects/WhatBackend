using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class ChangeCurrentPasswordDtoValidator : AbstractValidator<ChangeCurrentPasswordDto>
    {
        public ChangeCurrentPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(50).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("Password cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("Password cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.NewPassword).WithMessage("Passwords does not match");
        }
    }
}
