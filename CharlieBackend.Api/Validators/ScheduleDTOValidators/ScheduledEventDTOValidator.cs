using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class ScheduledEventDTOValidator : AbstractValidator<ScheduledEventDTO>
    {
        public ScheduledEventDTOValidator() //Is not necessary
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.EventOccuranceId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ThemeId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.MentorId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.LessonId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.EventStart)
                .NotEmpty();
            RuleFor(x => x.EventFinish)
                .Must((x, cancellation) => (x.EventFinish > x.EventStart || x.EventFinish.Equals(x.EventStart)))
                .When(x => x.EventFinish != null)
                .WithMessage(ValidationConstants.DatesNotValid);
            
        }
    }
}
