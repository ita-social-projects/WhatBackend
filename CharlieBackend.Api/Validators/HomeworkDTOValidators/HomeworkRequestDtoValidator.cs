using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Homework;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkDTOValidators
{
    public class HomeworkRequestDtoValidator : AbstractValidator<HomeworkRequestDto>
    {
        public HomeworkRequestDtoValidator()
        {
            RuleFor(x => x.DueDate)
                .NotEmpty();
            RuleFor(x => x.TaskText)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthTaskText);
            RuleFor(x => x.LessonId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.AttachmentIds)
                .NotEmpty();
            RuleForEach(x => x.AttachmentIds)
                .NotEmpty();
        }
    }
}

