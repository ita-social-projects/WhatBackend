using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Mentor;
using FluentValidation;

namespace CharlieBackend.Api.Validators.MentorDTOValidators
{
    /// <summary>
    /// UpdateMentorDtoValidator fluent validator
    /// </summary>
    public class UpdateMentorDtoValidator : AbstractValidator<UpdateMentorDto>
    {
        /// <summary>
        /// Fluent validation rules for UpdateStudentDto
        /// </summary>
        public UpdateMentorDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .Matches(ValidationConstants.EmailRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthEmail);
            RuleFor(x => x.FirstName)
                .MinimumLength(ValidationConstants.MinLengthName)
                .Matches(ValidationConstants.NameRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleFor(x => x.LastName)
                .MinimumLength(ValidationConstants.MinLengthName)
                .Matches(ValidationConstants.NameRegexCheck)
                .MaximumLength(ValidationConstants.MaxLengthName);
            RuleForEach(x => x.CourseIds)
                .NotNull()
                .GreaterThan(0);
            RuleForEach(x => x.StudentGroupIds)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
