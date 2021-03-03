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
                .MaximumLength(50).WithMessage("Email cannot be greateh than {MaxLength} symbols");
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(30)
                .Must(PasswordHelper.PasswordValidation).WithMessage("Password must have at least eight characters, at least one uppercase letter, one lowercase letter and one number");
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
