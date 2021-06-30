using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class EventOccurrenceDTOValidator : AbstractValidator<EventOccurrenceDTO>
    {
        public EventOccurrenceDTOValidator() //Is not necessary
        {
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.EventStart)
                .NotEmpty();
            RuleFor(x => x.EventFinish)
                .Must((x, cancellation) => (x.EventFinish > x.EventStart || x.EventFinish.Equals(x.EventStart)))
                .When(x => x.EventFinish != null)
                .WithMessage(ValidationConstants.DatesNotValid);
            RuleFor(x => x.Pattern)
                .NotEmpty();
            RuleFor(x => x.Events)
                .NotNull();
            RuleForEach(x => x.Events)
                .NotNull();
            RuleFor(x => x.Storage)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
