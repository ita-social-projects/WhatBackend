using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Student;
using FluentValidation;

namespace CharlieBackend.Api.Validators.StudentDTOValidators
{
    /// <summary>
    /// UpdateStudentDtoValidator fluent validator
    /// </summary>
    public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
    {
        /// <summary>
        /// Fluent validation rules for UpdateStudentDto
        /// </summary>
        public UpdateStudentDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .Matches(ValidationConstants.EmailRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
                .MinimumLength(ValidationConstants.MinLengthName)
                .MaximumLength(ValidationConstants.MaxLengthName)
                .Matches(ValidationConstants.NameRegexCheck);
            RuleFor(x => x.LastName)
                .MinimumLength(ValidationConstants.MinLengthName)
                .MaximumLength(ValidationConstants.MaxLengthName)
                .Matches(ValidationConstants.NameRegexCheck);
            RuleForEach(x => x.StudentGroupIds)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}