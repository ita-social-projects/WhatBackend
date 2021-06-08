using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;
using System;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentsClassbookDTOValidators
{
    public class StudentMarkDtoValidator : AbstractValidator<StudentMarkDto>
    {
        public StudentMarkDtoValidator()
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
            RuleFor(x => x.LessonId)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.LessonDate)
                .NotNull()
                .Must(x => BeAValidDate(x));
            RuleFor(x => x.StudentMark)
                .NotNull()
                .GreaterThanOrEqualTo((sbyte)0)
                .LessThanOrEqualTo((sbyte)100);
            RuleFor(x => x.Comment)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
        }
        private bool BeAValidDate(DateTime? date)
        {
            return !date.Equals(default(DateTime?));
        }
    }
}
