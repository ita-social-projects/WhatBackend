using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Course;
using FluentValidation;

namespace CharlieBackend.Api.Validators.CourseDTOValidators
{
    public class UpdateCourseDTOValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxLengthHeader);
        }
    }
}