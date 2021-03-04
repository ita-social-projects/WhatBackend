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
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(50);
            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .MaximumLength(30);
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword);
        }
    }
}
