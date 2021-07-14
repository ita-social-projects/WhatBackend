using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Lesson;
using FluentValidation;

namespace CharlieBackend.Api.Validators.LessonDTOValidators
{
    public class CreateLessonDtoValidator : AbstractValidator<CreateLessonDto>
    {
        public CreateLessonDtoValidator()
        {
            RuleFor(x => x.ThemeName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthHeader);
            RuleFor(x => x.MentorId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
               .NotEmpty()
               .GreaterThan(0);
            RuleFor(x => x.LessonVisits)
               .NotNull();
            RuleForEach(x => x.LessonVisits)
               .NotNull();
//               .SetValidator(VisitDtoValidator); uncomment after all validators are up and running
            RuleFor(x => x.LessonDate)
               .NotEmpty();
        }
    }
}
