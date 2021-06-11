using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;


namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentsClassbookDTOValidators
{
    public class StudentsClassbookResultDtoValidator : AbstractValidator<StudentsClassbookResultDto>
    {
        public StudentsClassbookResultDtoValidator()
    {
        RuleForEach(x => x.StudentsMarks)
            .NotNull();
        RuleFor(x => x.StudentsMarks)
            .NotNull();
        RuleForEach(x => x.StudentsPresences)
            .NotNull();
        RuleFor(x => x.StudentsPresences)
            .NotNull();
    }
}
}
