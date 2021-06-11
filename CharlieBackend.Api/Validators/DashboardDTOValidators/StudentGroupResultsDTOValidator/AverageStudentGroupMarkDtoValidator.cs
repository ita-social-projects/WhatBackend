using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentGroupResultsDTOValidator
{
    public class AverageStudentGroupMarkDtoValidator : AbstractValidator<AverageStudentGroupMarkDto>
    {
        public AverageStudentGroupMarkDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.AverageMark)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
        }
    }
}
