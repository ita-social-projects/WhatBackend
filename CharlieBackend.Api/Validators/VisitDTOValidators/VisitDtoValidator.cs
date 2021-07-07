using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Visit;
using FluentValidation;

namespace CharlieBackend.Api.Validators.VisitDTOValidators
{
    public class VisitDtoValidator : AbstractValidator<VisitDto>
    {
        public VisitDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.StudentMark)
                .GreaterThanOrEqualTo((sbyte)0);
            RuleFor(x => x.Presence)
                .NotNull();
            RuleFor(x => x.Comment)
                .MaximumLength(ValidationConstants.MaxLengthCommentText);

        }
    }
}