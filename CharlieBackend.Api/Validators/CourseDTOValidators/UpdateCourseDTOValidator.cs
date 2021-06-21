using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Course;
using FluentValidation;

namespace CharlieBackend.Api.Validators.CourseDTOValidators
{
    public class UpdateCourseDTOValidator : AbstractValidator<CreateCourseDto>
    {
        public UpdateCourseDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthName);
        }
    }
}