using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    /// <summary>
    /// ResetPasswordDtoValidator fluent validator
    /// </summary>
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        /// <summary>
        /// Fluent validation rules for ResetPasswordDto
        /// </summary>
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .MinimumLength(ValidationConstants.MinLength)
               .MaximumLength(ValidationConstants.MaxLengthPassword)
               .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants.PasswordRule);
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword).WithMessage(ValidationConstants.PasswordConfirmNotValid);
        }
    }
}
