using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    /// <summary>
    /// ContextForCreateScheduleDTOValidator fluent validator
    /// </summary>
    public class ContextForCreateScheduleDTOValidator : AbstractValidator<ContextForCreateScheduleDTO>
    {
        /// <summary>
        /// Fluent validation rules for ContextForCreateScheduleDTO
        /// </summary>
        public ContextForCreateScheduleDTOValidator()
        {
            RuleFor(x => x.GroupID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ThemeID)
                .GreaterThan(0);
            RuleFor(x => x.MentorID)
                .GreaterThan(0);
            RuleFor(x => x.ColorID).
                GreaterThan(0);
        }
    }
}
