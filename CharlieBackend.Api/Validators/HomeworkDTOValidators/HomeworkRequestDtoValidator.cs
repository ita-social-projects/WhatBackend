using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Homework;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkDTOValidators
{
    public class HomeworkRequestDtoValidator : AbstractValidator<HomeworkRequestDto>
    {
        public HomeworkRequestDtoValidator()
        {
            RuleFor(x => x.TaskText)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthText);
            RuleFor(x => x.LessonId)
                .NotEmpty()
                .GreaterThan(0);
            RuleForEach(x => x.AttachmentIds)
                .NotNull()
                .GreaterThan(0);
        }
    }
}

