using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class AuthenticationDtoValidator : AbstractValidator<AuthenticationDto>
    {
        public AuthenticationDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(ValidationConstants.MinLength)
                .MaximumLength(ValidationConstants.MaxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants.PasswordRule);
        }
    }
}
