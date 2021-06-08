using FluentValidation;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Attachment;
using System;

namespace CharlieBackend.Api.Validators.AttachmentDTOValidators
{
    public class AttachmentDtoValidator : AbstractValidator<AttachmentDto>
    {
        public AttachmentDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.CreatedOn)
                .NotEmpty()
                .Must(BeAValidDate);
            RuleFor(x => x.CreatedByAccountId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.ContainerName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
