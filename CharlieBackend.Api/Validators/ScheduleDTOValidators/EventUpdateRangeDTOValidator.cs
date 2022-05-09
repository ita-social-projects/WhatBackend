using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    /// <summary>
    /// EventUpdateRangeDTOValidator fluent validator
    /// </summary>
    public class EventUpdateRangeDTOValidator : AbstractValidator<EventUpdateRangeDTO>
    {
        /// <summary>
        /// Fluent validation rules for EventUpdateRangeDTO
        /// </summary>
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
