using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Mentor;
using FluentValidation;

namespace CharlieBackend.Api.Validators.MentorDTOValidators
{
    public class MentorDtoValidator : AbstractValidator<MentorDto>
    {
        public MentorDtoValidator() //Is not necessary
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
               .NotEmpty()
               .MaximumLength(ValidationConstants.MaxLengthName);
        }
    }
}
