using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Mentor;
using FluentValidation;

namespace CharlieBackend.Api.Validators.MentorDTOValidators
{
    public class MentorStudyGroupsDtoValidator : AbstractValidator<MentorStudyGroupsDto> 
    {
        public MentorStudyGroupsDtoValidator() //Is not necessary
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Name)
               .NotEmpty()
               .MaximumLength(ValidationConstants.MaxLengthHeader);
        }
    }
}
