using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using CharlieBackend.Core.DTO.Account;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(50).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.FormUrl)
                .NotEmpty().WithMessage("{PropertyName} is required")
                //check url
                .MaximumLength(200).WithMessage("Url cannot be greateh than {MaxLength} symbols");
        }

    }
}
