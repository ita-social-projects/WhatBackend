using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Course;
using CharlieBackend.Core.DTO.EmailData;
using FluentValidation;

namespace CharlieBackend.Api.Validators.EmailDataDTOValidator
{
    public class EmailDataDtoValidator : AbstractValidator<EmailData>
    {
        public EmailDataDtoValidator()
        {
            RuleFor(x => x.RecipientMail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.EmailBody)
                .NotEmpty();
        }
    }
}
