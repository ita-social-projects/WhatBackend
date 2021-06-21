using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;


namespace CharlieBackend.Api.Validators.Schedule.CreateScheduleDTO
{
    public class CreateScheduleDtoValidator : AbstractValidator<CreateScheduleDto>
    {
        public CreateScheduleDtoValidator()
        {
            RuleFor(x => x.Pattern)
                .NotEmpty();
            RuleFor(x => x.Range)
                .NotEmpty();
            RuleFor(x => x.Context)
                .NotEmpty();
        }
    }
}
