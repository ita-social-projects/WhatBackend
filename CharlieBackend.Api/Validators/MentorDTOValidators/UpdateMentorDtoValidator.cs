using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Mentor;
using FluentValidation;

namespace CharlieBackend.Api.Validators.MentorDTOValidators
{
    public class UpdateMentorDtoValidator : AbstractValidator<UpdateMentorDto>
    {
        public UpdateMentorDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
               .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
               .MaximumLength(ValidationConstants.MaxLengthName);
            RuleForEach(x => x.CourseIds)
               .NotNull()
               .GreaterThan(0);
            RuleForEach(x => x.StudentGroupIds)
               .NotNull()
               .GreaterThan(0);
        }
    }
}
