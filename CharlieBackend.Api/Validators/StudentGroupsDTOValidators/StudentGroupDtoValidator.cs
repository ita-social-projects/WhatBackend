using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.StudentGroups;
using FluentValidation;

namespace CharlieBackend.Api.Validators.StudentGroupsDTOValidators
{
    public class StudentGroupDtoValidator : AbstractValidator<StudentGroupDto>
    {
        public StudentGroupDtoValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty()
               .GreaterThan(0);
            RuleFor(x => x.CourseId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Name)
                .MaximumLength(ValidationConstants.MaxLengthHeader);
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(SharedResources.DatesNotValidMessage);
            RuleForEach(x => x.StudentIds)
                .NotEmpty()
                .GreaterThan(0);
            RuleForEach(x => x.MentorIds)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}