using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    /// <summary>
    /// ChangeCurrentPasswordDtoValidator fluent validator
    /// </summary>
    public class ChangeCurrentPasswordDtoValidator : AbstractValidator<ChangeCurrentPasswordDto>
    {
        /// <summary>
        /// Fluent validation rules for ChangeCurrentPasswordDto
        /// </summary>
        public ChangeCurrentPasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(ValidationConstants.MinLength)
                .MaximumLength(ValidationConstants.MaxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(SharedResources.PasswordRuleWarningMessage);
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(ValidationConstants.MinLength)
                .MaximumLength(ValidationConstants.MaxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(SharedResources.PasswordRuleWarningMessage);
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword).WithMessage(SharedResources.PasswordNotValidMessage);
        }
    }
}
