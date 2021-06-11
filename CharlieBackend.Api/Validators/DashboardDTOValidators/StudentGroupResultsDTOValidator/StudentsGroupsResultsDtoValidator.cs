using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentGroupResultsDTOValidator
{
    public class StudentsGroupsResultsDtoValidator : AbstractValidator<StudentGroupsResultsDto>
    {
        public StudentsGroupsResultsDtoValidator()
        {
            RuleForEach(x => x.AverageStudentGroupsVisits)
                .NotNull();
            RuleFor(x => x.AverageStudentGroupsVisits)
                .NotNull();
            RuleForEach(x => x.AverageStudentGroupsMarks)
                .NotNull();
            RuleFor(x => x.AverageStudentGroupsMarks)
                .NotNull();
        }
    }
}
