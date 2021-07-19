using CharlieBackend.Core.DTO.Lesson;
using FluentValidation;

namespace CharlieBackend.Api.Validators.LessonDTOValidators
{
    public class AssignMentorToLessonDtoValidator : AbstractValidator<AssignMentorToLessonDto>
    {
        public AssignMentorToLessonDtoValidator()
        {
            RuleFor(x => x.MentorId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.LessonId)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
