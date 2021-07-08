using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Theme;
using FluentValidation;

namespace CharlieBackend.Api.Validators.ThemeDTOValidators
{
    public class UpdateThemeDtoValidator : AbstractValidator<UpdateThemeDto>
    {
        public UpdateThemeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthHeader);
        }
    }
}