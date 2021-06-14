using FluentValidation;
using CharlieBackend.Core.DTO.Dashboard;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentsResultsDTOValidators
{
    public class AverageStudentVisitsDtoValidator : AbstractValidator<AverageStudentVisitsDto>
    {
        public AverageStudentVisitsDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StudentGroupId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StudentId)
               .NotNull()
               .GreaterThan(0);
            RuleFor(x => x.StudentAverageVisitsPercentage)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
        }
    }
}
