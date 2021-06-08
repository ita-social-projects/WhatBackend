using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Dashboard;
using FluentValidation;
using System;

namespace CharlieBackend.Api.Validators.DashboardDTOValidators.StudentsClassbookDTOValidators
{
    public class StudentVisitDtoValidator : AbstractValidator<StudentVisitDto>
    {
        public StudentVisitDtoValidator()
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
                .NotEmpty();
            RuleFor(x => x.Presence)
                .NotNull();
        }
    }
}