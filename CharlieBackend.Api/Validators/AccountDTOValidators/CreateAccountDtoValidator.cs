using CharlieBackend.Core.DTO.Account;
using FluentValidation;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("{PropertyName} is required")
                 .MaximumLength(ValidationConstants.MaxLengthName).WithMessage("{PropertyName} can't be greater than {MaxLength} symbols");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(ValidationConstants.MaxLengthName).WithMessage("{PropertyName} can't be greater than {MaxLength} symbols");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(ValidationConstants.MaxLengthEmail).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(ValidationConstants.MinLength)
                .MaximumLength(ValidationConstants.MaxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants.PasswordRule);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.Password).WithMessage(ValidationConstants.PasswordConfirmNotValid);
            
        }
    }
}
