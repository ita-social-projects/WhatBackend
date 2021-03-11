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
