using CharlieBackend.Core.DTO.Account;
using FluentValidation;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class AuthenticationDtoValidator : AbstractValidator<AuthenticationDto>
    {
        public AuthenticationDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(ValidationConstants.MaxLengthEmail).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(ValidationConstants.MinLength)
                .MaximumLength(ValidationConstants.MaxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants.PasswordRule);
        }
    }
}
