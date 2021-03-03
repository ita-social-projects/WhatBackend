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
                .MaximumLength(ValidationConstants._maxLengthEmail).WithMessage("Email cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .MinimumLength(ValidationConstants._minLength)
               .MaximumLength(ValidationConstants._maxLengthPassword)
               .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants._passwordRule);
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.NewPassword).WithMessage(ValidationConstants._passwordConfirmNotValid);
        }
    }
}
