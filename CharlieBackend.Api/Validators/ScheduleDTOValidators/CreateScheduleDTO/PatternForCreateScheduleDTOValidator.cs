using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    /// <summary>
    /// PatternForCreateScheduleDTOValidator fluent validator
    /// </summary>
    public class PatternForCreateScheduleDTOValidator : AbstractValidator<PatternForCreateScheduleDTO>
    {
        /// <summary>
        /// Fluent validation rules for PatternForCreateScheduleDTO
        /// </summary>
        public PatternForCreateScheduleDTOValidator()
        {
            RuleFor(x => x.Type)
                .NotNull();
            RuleFor(x => x.Interval)
                .NotEmpty()
                .GreaterThan(0);
            RuleForEach(x => x.DaysOfWeek)
                .NotNull();
            RuleForEach(x => x.Dates)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(31);
        }
    }
}
