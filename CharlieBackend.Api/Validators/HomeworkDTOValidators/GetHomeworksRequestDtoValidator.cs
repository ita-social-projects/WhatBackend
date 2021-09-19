using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Homework;
using FluentValidation;

namespace CharlieBackend.Api.Validators.HomeworkDTOValidators
{
    public class GetHomeworksRequestDtoValidator : AbstractValidator<GetHomeworkRequestDto>
    {
        public GetHomeworksRequestDtoValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0);
            RuleFor(x => x.CourseId)
                .GreaterThan(0);
            RuleFor(x=> x.ThemeId)
                .GreaterThan(0);
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);
        }
    }
}