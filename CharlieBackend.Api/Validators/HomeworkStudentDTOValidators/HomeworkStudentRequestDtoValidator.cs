using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.HomeworkStudent;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkStudentDTOValidators
{
    public class HomeworkStudentRequestDtoValidator : AbstractValidator<HomeworkStudentRequestDto>
    {
        public HomeworkStudentRequestDtoValidator()
        {
            RuleFor(x => x.HomeworkId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.HomeworkText)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthTaskText);
            RuleForEach(x => x.AttachmentIds)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.AttachmentIds)
                .NotNull();
        }
    }
}
