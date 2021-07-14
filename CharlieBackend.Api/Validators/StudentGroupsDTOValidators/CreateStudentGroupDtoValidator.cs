using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.StudentGroups;
using FluentValidation;

namespace CharlieBackend.Api.Validators.StudentGroupsDTOValidators
{
    public class CreateStudentGroupDtoValidator : AbstractValidator<CreateStudentGroupDto>
    {
        public CreateStudentGroupDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthHeader);
            RuleFor(x => x.CourseId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StartDate)
               .NotEmpty();
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);
            RuleFor(x => x.StudentIds)
                .NotEmpty();
            RuleForEach(x => x.StudentIds)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.MentorIds)
                .NotEmpty();
            RuleForEach(x => x.MentorIds)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}