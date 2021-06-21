using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class UpdateScheduledEventDtoValidator : AbstractValidator<UpdateScheduledEventDto>
    {
        public UpdateScheduledEventDtoValidator()
        {
            RuleFor(x => x.StudentGroupId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ThemeId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.MentorId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.EventStart)
                .NotEmpty();
            RuleFor(x => x.EventEnd)
                .Must((x, cancellation) => (x.EventEnd > x.EventStart || x.EventEnd.Equals(x.EventStart)))
                .When(x => x.EventEnd != null)
                .WithMessage(ValidationConstants.DatesNotValid);

        }
    }
}
