using System;
using FluentValidation;
using CharlieBackend.Core.DTO.Dashboard;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentsResultsDTOValidators
{
    public class StudentsResultsDtoValidator : AbstractValidator<StudentsResultsDto>
    {
        public StudentsResultsDtoValidator()
        {
            RuleForEach(x => x.AverageStudentVisits)
                .NotNull();
            RuleFor(x => x.AverageStudentVisits)
                .NotNull();
            RuleForEach(x => x.AverageStudentsMarks)
                .NotNull();
            RuleFor(x => x.AverageStudentsMarks)
                .NotNull();
        }
    }
}
