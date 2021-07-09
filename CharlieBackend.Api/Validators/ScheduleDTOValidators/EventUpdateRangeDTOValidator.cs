using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class EventUpdateRangeDTOValidator : AbstractValidator<EventUpdateRangeDTO>
    {
        public EventUpdateRangeDTOValidator()
        {
            RuleFor(x => x.Filter)
                .NotEmpty()
                .SetValidator(new ScheduledEventFilterRequestDTOValidator());
            RuleFor(x => x.Request)
                .NotEmpty()
                .SetValidator(new UpdateScheduledEventDtoValidator());
        }
    }
}
