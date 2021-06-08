using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Course;
using FluentValidation;


namespace CharlieBackend.Api.Validators.CourseDTOValidators
{
    public class CourseDtoValidator : AbstractValidator<CourseDto>
    {
        public CourseDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.IsActive)
                .NotNull();
        }
    }
}
