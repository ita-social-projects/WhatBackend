﻿using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Homework;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkDTOValidators
{
    public class GetHomeworksRequestDtoValidator : AbstractValidator<GetHomeworkRequestDto>
    {
        public GetHomeworksRequestDtoValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .Must((x, c) => x.GroupId.HasValue).When(x => !x.CourseId.HasValue && !x.ThemeId.HasValue)
                .WithMessage(Resources.Resource.GroupIdHasNoValueWarningMessage);
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .Must((x, c) => x.CourseId.HasValue).When(x => !x.GroupId.HasValue && !x.ThemeId.HasValue)
                .WithMessage(Resources.Resource.CourseIdHasNoValueWarningMessage);
            RuleFor(x=> x.ThemeId)
                .GreaterThan(0)
                .Must((x, c) => x.ThemeId.HasValue).When(x => !x.CourseId.HasValue && !x.GroupId.HasValue)
                .WithMessage(Resources.Resource.ThemeIdHasNoValueWarningMessage);
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(SharedResources.DatesNotValidMessage);
        }
    }
}