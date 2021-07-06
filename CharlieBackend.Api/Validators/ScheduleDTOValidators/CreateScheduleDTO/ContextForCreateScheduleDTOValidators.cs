using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    public class ContextForCreateScheduleDTOValidators : AbstractValidator<ContextForCreateScheduleDTO>
    {
        public ContextForCreateScheduleDTOValidators()
        {
            RuleFor(x => x.GroupID)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ThemeID)
                .GreaterThan(0);
            RuleFor(x => x.MentorID)
                .GreaterThan(0);
        }
    }
}
