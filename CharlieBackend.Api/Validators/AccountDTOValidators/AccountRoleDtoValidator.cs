using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    /// <summary>
    /// AccountRoleDtoValidator fluent validator
    /// </summary>
    public class AccountRoleDtoValidator : AbstractValidator<AccountRoleDto>
    {
        /// <summary>
        /// Fluent validation rules for AccountRoleDto
        /// </summary>
        public AccountRoleDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);              
        }
    }
}
