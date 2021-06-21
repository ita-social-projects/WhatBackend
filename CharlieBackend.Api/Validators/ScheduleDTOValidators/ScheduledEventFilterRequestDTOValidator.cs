using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class ScheduledEventFilterRequestDTOValidator : AbstractValidator<ScheduledEventFilterRequestDTO>
    {
        public ScheduledEventFilterRequestDTOValidator()
        {
            RuleFor(x => x.CourseID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.MentorID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.GroupID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ThemeID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentAccountID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.EventOccurrenceID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StartDate)
                .NotEmpty();
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);

        }
    }
}
