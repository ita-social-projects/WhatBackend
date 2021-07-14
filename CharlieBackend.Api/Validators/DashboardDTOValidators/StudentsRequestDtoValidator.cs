using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;
using System;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators
{
    public class StudentsRequestDtoValidator<T> : AbstractValidator<StudentsRequestDto<T>> where T : Enum
    {
        public StudentsRequestDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .GreaterThan(0);
            RuleFor(x => x.StartDate)
                .NotEmpty();
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);
            RuleFor(x => x.IncludeAnalytics)
                .NotNull();

        }
    }
}
