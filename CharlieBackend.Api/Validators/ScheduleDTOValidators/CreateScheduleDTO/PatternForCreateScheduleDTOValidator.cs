using CharlieBackend.Business.Helpers;
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
                .NotNull()
                .IsInEnum();
            RuleFor(x => x.Interval)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.DaysOfWeek)
                .NotNull()
                .NotEmpty();
            RuleForEach(x => x.DaysOfWeek)
                .IsInEnum();
            RuleFor(x => x.Dates)
                .NotNull()
                .NotEmpty();
            RuleForEach(x => x.Dates)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(31);
            RuleFor(x => x.Index)
                .IsInEnum();
        }
    }
}
