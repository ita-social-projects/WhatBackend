using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Theme;
using FluentValidation;

namespace CharlieBackend.Api.Validators.ThemeDTOValidators
{
    public class CreateThemeDtoValidator : AbstractValidator<CreateThemeDto>
    {
        public CreateThemeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthHeader);
        }
    }
}