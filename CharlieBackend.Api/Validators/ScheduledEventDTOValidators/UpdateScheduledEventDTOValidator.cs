using CharlieBackend.Business.Exceptions;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Validators.ScheduledEventDTOValidators
{
    public class UpdateScheduledEventDTOValidator : AbstractValidator<UpdateScheduledEventDto>
    {
        public UpdateScheduledEventDTOValidator()
        {
            RuleFor(x => x.StudentGroupId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.ThemeId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.MentorId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.EventEnd)
                .Must((x, cancellation) => x.EventStart.HasValue && x.EventEnd.HasValue
                    && (x.EventEnd > x.EventStart || x.EventEnd.Equals(x.EventStart)))
                .When(x => x.EventEnd != null)
                .WithMessage(ValidationConstants.DatesNotValid);
        }
    }
}
