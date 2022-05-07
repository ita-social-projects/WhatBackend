using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    /// <summary>
    /// AuthenticationDtoValidator fluent validator
    /// </summary>
    public class AuthenticationDtoValidator : AbstractValidator<AuthenticationDto>
    {
        /// <summary>
        /// Fluent validation rules for AuthenticationDto
        /// </summary>
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
