using System;
using System.Collections.Generic;
using System.Text;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Core.Validators.AccountDTOValidators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
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
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(65).WithMessage("Password cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.Password).WithMessage("Passwords does not match");
            
        }
    }
}
