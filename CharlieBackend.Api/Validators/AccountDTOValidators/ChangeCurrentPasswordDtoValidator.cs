using CharlieBackend.Core.DTO.Account;
using FluentValidation;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class ChangeCurrentPasswordDtoValidator : AbstractValidator<ChangeCurrentPasswordDto>
    {
        public ChangeCurrentPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(ValidationConstants._maxLengthEmail).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(ValidationConstants._minLength)
                .MaximumLength(ValidationConstants._maxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(ValidationConstants._passwordRule);
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
