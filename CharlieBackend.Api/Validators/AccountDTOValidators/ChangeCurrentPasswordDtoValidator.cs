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
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(50);
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MaximumLength(30);
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MaximumLength(30);
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword).WithMessage("Passwords does not match");
        }
    }
}
