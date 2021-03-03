using CharlieBackend.Core.DTO.Account;
using FluentValidation;
using CharlieBackend.Business.Helpers;

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
                 .MaximumLength(ValidationConstants._maxLengthName).WithMessage("{PropertyName} can't be greater than {MaxLength} symbols");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(ValidationConstants._maxLengthName).WithMessage("{PropertyName} can't be greater than {MaxLength} symbols");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(ValidationConstants._maxLengthEmail).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .IsInEnum().WithMessage("Invalid role");
            RuleFor(x => x.IsActive)
                .NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
