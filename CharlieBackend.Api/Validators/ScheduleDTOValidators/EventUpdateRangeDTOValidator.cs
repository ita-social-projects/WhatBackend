using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;
using System;

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
