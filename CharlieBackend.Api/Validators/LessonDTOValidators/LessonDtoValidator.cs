using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Lesson;
using FluentValidation;

namespace CharlieBackend.Api.Validators.LessonDTOValidators
{
    public class LessonDtoValidator : AbstractValidator<LessonDto>
    {
        public LessonDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ThemeName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.MentorId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.LessonDate)
                .NotEmpty();
            RuleFor(x => x.LessonVisits)
                .NotNull();
            RuleForEach(x => x.LessonVisits)
                .NotNull();

        }
    }
}