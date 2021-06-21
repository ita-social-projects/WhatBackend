using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    public class PatternForCreateScheduleDTOValidator : AbstractValidator<PatternForCreateScheduleDTO>
    {
        public PatternForCreateScheduleDTOValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty();
            RuleFor(x => x.Interval)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.DaysOfWeek)
                .NotEmpty();
            RuleForEach(x => x.DaysOfWeek)
                .NotEmpty();
            RuleFor(x => x.DaysOfWeek)
                .NotEmpty();
        }
    }
}
