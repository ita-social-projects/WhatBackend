using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class EventUpdateRangeDTOValidator : AbstractValidator<EventUpdateRangeDTO>
    {
        public EventUpdateRangeDTOValidator()
        {
            RuleFor(x => x.Filter)
                .NotEmpty();
            RuleFor(x => x.Request)
                .NotEmpty();
        }
    }
}
