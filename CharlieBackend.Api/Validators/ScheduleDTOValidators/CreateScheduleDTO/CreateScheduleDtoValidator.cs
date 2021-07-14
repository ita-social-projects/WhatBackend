using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    public class CreateScheduleDtoValidator : AbstractValidator<CreateScheduleDto>
    {
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
