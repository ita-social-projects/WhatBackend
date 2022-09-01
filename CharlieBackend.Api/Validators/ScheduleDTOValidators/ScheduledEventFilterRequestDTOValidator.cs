using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    /// <summary>
    /// ScheduledEventFilterRequestDTOValidator fluent validator
    /// </summary>
    public class ScheduledEventFilterRequestDTOValidator : AbstractValidator<ScheduledEventFilterRequestDTO>
    {
        /// <summary>
        /// Fluent validation rules for ScheduledEventFilterRequestDTO
        /// </summary>
        public ScheduledEventFilterRequestDTOValidator()
        {
            RuleFor(x => x.CourseID)
                .GreaterThan(0);
            RuleFor(x => x.MentorID)
                .GreaterThan(0);
            RuleFor(x => x.GroupID)
                .GreaterThan(0);
            RuleFor(x => x.ThemeID)
                .GreaterThan(0);
            RuleFor(x => x.StudentAccountID)
                .GreaterThan(0);
            RuleFor(x => x.EventOccurrenceID)
                .GreaterThan(0);
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(SharedResources.DatesNotValidMessage);

        }
    }
}
