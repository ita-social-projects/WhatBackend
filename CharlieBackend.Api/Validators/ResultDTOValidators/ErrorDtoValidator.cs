using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Result;
using FluentValidation;

namespace CharlieBackend.Api.Validators.ResultDTOValidators
{
    public class ErrorDtoValidator : AbstractValidator<ErrorDto>
    {
        public ErrorDtoValidator() //Is not necessary
        {
            RuleFor(x => x.Error)
               .NotEmpty();
        }
    }
}
