using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.ScheduledEventDTOValidators
{
    public class UpdateScheduledEventDTOValidator : AbstractValidator<UpdateScheduledEventDto>
    {
        public UpdateScheduledEventDTOValidator()
        {
            RuleFor(x => x.StudentGroupId)
                .GreaterThan(0);

            RuleFor(x => x.ThemeId)
                .GreaterThan(0);

            RuleFor(x => x.MentorId)
                .GreaterThan(0);

            RuleFor(x => x.EventEnd)
                .Must((x, cancellation) => (x.EventEnd > x.EventStart || x.EventEnd.Equals(x.EventStart)))
                .When(x => x.EventEnd != null)
                .WithMessage(SharedResources.DatesNotValidMessage);
        }
    }
}
