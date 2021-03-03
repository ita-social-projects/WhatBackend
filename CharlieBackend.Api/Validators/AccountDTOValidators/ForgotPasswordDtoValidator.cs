using System;
using FluentValidation;
using CharlieBackend.Core.DTO.Account;
using CharlieBackend.Business.Helpers;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Incorrect email")
                .MaximumLength(ValidationConstants._maxLengthEmail).WithMessage("Email cannot be greater than {MaxLength} symbols");
            RuleFor(x => x.FormUrl)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(ValidationConstants._maxLengthURL).WithMessage("Url cannot be greater than {MaxLength} symbols")
                .Must(BeValidURL);
        }

        protected bool BeValidURL(string URL)
        {
            Uri uriResult;
            return Uri.TryCreate(URL, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
