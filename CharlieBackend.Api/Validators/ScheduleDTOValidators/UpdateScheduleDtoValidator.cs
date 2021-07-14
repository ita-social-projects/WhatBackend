using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    public class UpdateScheduleDtoValidator : AbstractValidator<UpdateScheduleDto>
    {
        public UpdateScheduleDtoValidator()
        {
            RuleFor(x => x.LessonStart)
                .NotEmpty();
            RuleFor(x => x.LessonEnd)
                .Must((x, cancellation) => (x.LessonEnd > x.LessonStart || x.LessonEnd.Equals(x.LessonStart)))
                .When(x => x.LessonEnd != null)
                .WithMessage(ValidationConstants.DatesNotValid);
            RuleFor(x => x.RepeatRate)
                .NotEmpty();
            RuleFor(x => x.DayNumber)
                .GreaterThanOrEqualTo((uint)1)
                .LessThanOrEqualTo((uint)31);
        }
    }
}
