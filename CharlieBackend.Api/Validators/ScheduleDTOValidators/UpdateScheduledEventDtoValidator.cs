using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    /// <summary>
    /// UpdateScheduledEventDtoValidator fluent validator
    /// </summary>
    public class UpdateScheduledEventDtoValidator : AbstractValidator<UpdateScheduledEventDto>
    {
        /// <summary>
        /// Fluent validation rules for UpdateScheduledEventDto
        /// </summary>
        public UpdateScheduledEventDtoValidator()
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
