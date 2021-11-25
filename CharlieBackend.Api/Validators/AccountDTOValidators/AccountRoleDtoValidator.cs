using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class AccountRoleDtoValidator : AbstractValidator<AccountRoleDto>
    {
        public AccountRoleDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);              
        }
    }
}
