using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Secretary;
using FluentValidation;

namespace CharlieBackend.Api.Validators.SecretaryDTOValidators
{
    /// <summary>
    /// UpdateSecretaryDtoValidator fluent validator
    /// </summary>
    public class UpdateSecretaryDtoValidator : AbstractValidator<UpdateSecretaryDto>
    {
        /// <summary>
        /// Fluent validation rules for UpdateSecretaryDto
        /// </summary>
        public UpdateSecretaryDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .Matches(ValidationConstants.EmailRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
                .MinimumLength(ValidationConstants.MinLengthName)
                .Matches(ValidationConstants.NameRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
                .MinimumLength(ValidationConstants.MinLengthName)
                .Matches(ValidationConstants.NameRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthName);
        }
    }
}
