﻿using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Schedule;
using FluentValidation;

namespace CharlieBackend.Api.Validators.Schedule
{
    /// <summary>
    /// UpdateScheduleDtoValidator fluent validator
    /// </summary>
    public class UpdateScheduleDtoValidator : AbstractValidator<UpdateScheduleDto>
    {
        /// <summary>
        /// Fluent validation rules for UpdateScheduleDto
        /// </summary>
        public UpdateScheduleDtoValidator()
        {
            RuleFor(x => x.LessonStart)
                .NotEmpty();
            RuleFor(x => x.LessonEnd)
                .Must((x, cancellation) => (x.LessonEnd > x.LessonStart || x.LessonEnd.Equals(x.LessonStart)))
                .When(x => x.LessonEnd != null)
                .WithMessage(SharedResources.DatesNotValidMessage);
            RuleFor(x => x.RepeatRate)
                .NotEmpty()
                .IsInEnum();
            RuleFor(x => x.DayNumber)
                .GreaterThanOrEqualTo((uint)1)
                .LessThanOrEqualTo((uint)31);
        }
    }
}
