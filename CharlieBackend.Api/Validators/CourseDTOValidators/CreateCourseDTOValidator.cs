using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Course;
using FluentValidation;

namespace CharlieBackend.Api.Validators.CourseDTOValidators
{
    public class CreateCourseDTOValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthHeader);
        }
    }
}
