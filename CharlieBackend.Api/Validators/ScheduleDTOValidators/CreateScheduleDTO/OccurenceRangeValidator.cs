using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    public class OccurenceRangeValidator : AbstractValidator<OccurenceRange>
    {
        public OccurenceRangeValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty();
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null);
        }
    }
}
