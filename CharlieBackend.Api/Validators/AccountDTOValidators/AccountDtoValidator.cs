using CharlieBackend.Core.DTO.Account;
using FluentValidation;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class AccountDtoValidator : AbstractValidator<AccountDto>
    {
        public AccountDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.FirstName)
                 .NotEmpty()
                 .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.Role)
                .NotEmpty()
                .IsInEnum();
            RuleFor(x => x.IsActive)
                .NotEmpty();
        }
    }
}
