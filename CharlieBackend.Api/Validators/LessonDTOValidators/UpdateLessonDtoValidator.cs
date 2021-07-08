using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Lesson;
using FluentValidation;

namespace CharlieBackend.Api.Validators.LessonDTOValidators
{
    public class UpdateLessonDtoValidator : AbstractValidator<UpdateLessonDto>
    {
        public UpdateLessonDtoValidator()
        {
            RuleFor(x => x.ThemeName)
                .MaximumLength(ValidationConstants.MaxLengthHeader);
            RuleFor(x => x.LessonDate)
               .NotEmpty();
            RuleForEach(x => x.LessonVisits)
                .NotNull();
        }
    }
}