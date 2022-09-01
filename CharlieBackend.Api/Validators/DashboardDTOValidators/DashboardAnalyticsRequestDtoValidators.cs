using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;
using System;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators
{
    public class DashboardAnalyticsRequestDtoValidators<T> : AbstractValidator<DashboardAnalyticsRequestDto<T>> where T : Enum
    {
        public DashboardAnalyticsRequestDtoValidators()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty();
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(SharedResources.DatesNotValidMessage);
            RuleFor(x => x.IncludeAnalytics)
                .NotNull();
        }
    }
}
