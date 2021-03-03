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
                .MaximumLength(50).WithMessage("Email cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .MinimumLength(8)
               .MaximumLength(30)
               .Must(PasswordHelper.PasswordValidation).WithMessage("Password must have at least eight characters, at least one uppercase letter, one lowercase letter and one number");
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Equal(x => x.NewPassword).WithMessage("Passwords does not match");
        }
    }
}
