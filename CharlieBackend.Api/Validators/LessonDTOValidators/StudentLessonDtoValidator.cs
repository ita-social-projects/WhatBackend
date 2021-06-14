using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Lesson;
using FluentValidation;

namespace CharlieBackend.Api.Validators.LessonDTOValidators
{
    public class StudentLessonDtoValidator : AbstractValidator<StudentLessonDto>
    {
        public StudentLessonDtoValidator()
        {
            RuleFor(x => x.ThemeName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Presence)
                .NotNull();
            RuleFor(x => x.Mark)
                .NotNull()
                .GreaterThan((sbyte)0)
                .LessThan((sbyte)100);
            RuleFor(x => x.Comment)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthCommentText);
            RuleFor(x => x.StudentGroupId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.LessonDate)
               .NotEmpty();
        }
    }
}
