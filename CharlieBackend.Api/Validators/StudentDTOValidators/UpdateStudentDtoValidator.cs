using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Student;
using FluentValidation;

namespace CharlieBackend.Api.Validators.StudentDTOValidators
{
    public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
    {
        public UpdateStudentDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleForEach(x => x.StudentGroupIds)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}