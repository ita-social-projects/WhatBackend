using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentGroupResultsDTOValidator
{
    public class AverageStudentGroupVisitDtoValidator : AbstractValidator<AverageStudentGroupVisitDto>
    {
        public AverageStudentGroupVisitDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.AverageVisitPercentage)
                .NotNull()
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
        }
    }
}
