using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.HomeworkStudent;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkStudentDTOValidators
{
    public class HomeworkStudentDtoValidator : AbstractValidator<HomeworkStudentDto>
    {
        public HomeworkStudentDtoValidator() //IS not necessary
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
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

