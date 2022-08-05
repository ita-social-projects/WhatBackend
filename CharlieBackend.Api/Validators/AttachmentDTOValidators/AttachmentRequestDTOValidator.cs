using CharlieBackend.Business.Resources;
using CharlieBackend.Core.DTO.Attachment;
using FluentValidation;

namespace CharlieBackend.Api.Validators.AttachmentDTOValidators
{
    public class AttachmentRequestDTOValidator : AbstractValidator<AttachmentRequestDto>
    {
        public AttachmentRequestDTOValidator()
        {
            RuleFor(x => x.MentorID)
                .GreaterThan(0);
            RuleFor(x => x.CourseID)
                .GreaterThan(0);
            RuleFor(x => x.GroupID)
                .GreaterThan(0);
            RuleFor(x => x.StudentAccountID)
                .GreaterThan(0);
            RuleFor(x => x.FinishDate)
                .Must((x, cancellation) => (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(SharedResources.DatesNotValidMessage);
        }
    }
}
