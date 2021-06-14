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
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
               .NotEmpty()
               .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
               .NotEmpty()
               .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.CourseIds)
               .NotNull();
            RuleForEach(x => x.CourseIds)
               .NotNull()
               .GreaterThan(0);
            RuleFor(x => x.StudentGroupIds)
               .NotNull();
            RuleForEach(x => x.StudentGroupIds)
               .NotNull()
               .GreaterThan(0);
        }
    }
}
