using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Homework;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkDTOValidators
{
    public class HomeworkFilterDtoValidator : AbstractValidator<HomeworkFilterDto>
    {
        public HomeworkFilterDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);

            RuleFor(x => x.GroupId)
                .GreaterThan(0);

            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate)
                        || (x.FinishDate.Equals(x.StartDate))
                        || (x.StartDate == null))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);
        }
    }
}
