using FluentValidation;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(ValidationConstants.MaxLengthEmail).WithMessage("Email cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .MinimumLength(ValidationConstants.MinLength)
               .MaximumLength(ValidationConstants.MaxLengthPassword)
               .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants.PasswordRule);
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.NewPassword).WithMessage(ValidationConstants.PasswordConfirmNotValid);
        }
    }
}
