using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    /// <summary>
    /// CreateScheduleDtoValidator fluent validator
    /// </summary>
    public class CreateScheduleDtoValidator : AbstractValidator<CreateScheduleDto>
    {
        /// <summary>
        /// Fluent validation rules for CreateScheduleDto
        /// </summary>
        public CreateScheduleDtoValidator()
        {
            RuleFor(x => x.Pattern)
                .NotEmpty()
                .SetValidator(new PatternForCreateScheduleDTOValidator());
            RuleFor(x => x.Range)
                .NotEmpty()
                .SetValidator(new OccurenceRangeValidator());
            RuleFor(x => x.Context)
                .NotEmpty()
                .SetValidator(new ContextForCreateScheduleDTOValidator());
        }
    }
}
