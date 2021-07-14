using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Secretary;
using FluentValidation;

namespace CharlieBackend.Api.Validators.SecretaryDTOValidators
{
    public class UpdateSecretaryDtoValidator : AbstractValidator<UpdateSecretaryDto>
    {
        public UpdateSecretaryDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
                .MaximumLength(ValidationConstants.MaxLengthName);
        }
    }
}
