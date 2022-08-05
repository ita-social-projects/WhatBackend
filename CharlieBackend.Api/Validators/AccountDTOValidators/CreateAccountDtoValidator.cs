using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    /// <summary>
    /// CreateAccountDtoValidator fluent validator
    /// </summary>
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        /// <summary>
        /// Fluent validation rules for CreateAccountDto
        /// </summary>
        public CreateAccountDtoValidator()
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty()
                 .Matches(ValidationConstants.NameRegexCheck)
                 .MinimumLength(ValidationConstants.MinLengthName)
                 .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .Matches(ValidationConstants.NameRegexCheck)
                .MinimumLength(ValidationConstants.MinLengthName)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Matches(ValidationConstants.EmailRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(ValidationConstants.MinLength)
                .MaximumLength(ValidationConstants.MaxLengthPassword)
                .Must(PasswordHelper.PasswordValidation).WithMessage(SharedResources.PasswordRuleWarningMessage);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password).WithMessage(SharedResources.PasswordNotValidMessage);
        }
    }
}
