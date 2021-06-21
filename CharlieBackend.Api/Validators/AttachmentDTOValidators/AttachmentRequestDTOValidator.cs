using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Attachment;
using FluentValidation;
using System;

namespace CharlieBackend.Api.Validators.AttachmentDTOValidators
{
    public class AttachmentRequestDTOValidator : AbstractValidator<AttachmentRequestDto>
    {
        public AttachmentRequestDTOValidator()
        {
            RuleFor(x => x.MentorID)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.CourseID)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.GroupID)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StudentAccountID)
                .NotNull()
                .GreaterThan(0);
            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.FinishDate)
                   .Must((x, cancellation) => x.StartDate.HasValue && x.FinishDate.HasValue
                    && (x.FinishDate > x.StartDate || x.FinishDate.Equals(x.StartDate)))
                .When(x => x.FinishDate != null)
                .WithMessage(ValidationConstants.DatesNotValid);
        }
    }
}
