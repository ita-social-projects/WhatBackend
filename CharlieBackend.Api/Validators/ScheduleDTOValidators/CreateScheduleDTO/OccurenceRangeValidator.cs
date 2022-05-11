using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    /// <summary>
    /// OccurenceRangeValidator fluent validator
    /// </summary>
    public class OccurenceRangeValidator : AbstractValidator<OccurenceRange>
    {
        /// <summary>
        /// Fluent validation rules for OccurenceRange
        /// </summary>
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
