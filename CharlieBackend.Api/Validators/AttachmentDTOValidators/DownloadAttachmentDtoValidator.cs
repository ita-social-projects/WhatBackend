using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Attachment;
using FluentValidation;


namespace CharlieBackend.Api.Validators.AttachmentDTOValidators
{
    public class DownloadAttachmentDtoValidator : AbstractValidator<DownloadAttachmentDto>
    {
        public DownloadAttachmentDtoValidator()
        {
            RuleFor(x => x.DownloadInfo)
                .NotEmpty();
            RuleFor(x => x.FileName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
        }
    }
}
