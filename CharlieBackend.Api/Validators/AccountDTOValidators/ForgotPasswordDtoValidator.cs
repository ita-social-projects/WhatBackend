using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Account;
using FluentValidation;
using System;

namespace CharlieBackend.Api.Validators.AccountDTOValidators
{
    /// <summary>
    /// ForgotPasswordDtoValidator fluent validator
    /// </summary>
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        /// <summary>
        /// Fluent validation rules for ForgotPasswordDto
        /// </summary>
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FormUrl)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthURL)
                .Must(IsValidURL);
        }

        /// <summary>
        /// Checks if is url valid
        /// </summary>
        /// <param name="URL"></param>
        /// <returns>true if valid, false otherwise </returns>
        protected bool IsValidURL(string URL)
        {
            return Uri.TryCreate(URL, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
