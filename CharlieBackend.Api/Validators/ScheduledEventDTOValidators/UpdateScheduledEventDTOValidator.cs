using CharlieBackend.Business.Exceptions;
using CharlieBackend.Business.Helpers;
using CharlieBackend.Core.DTO.Schedule;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Validators.ScheduledEventDTOValidators
{
    public class UpdateScheduledEventDTOValidator : AbstractValidator<UpdateScheduledEventDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateScheduledEventDTOValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.StudentGroupId)
                            .MustAsync(async (StudentGroupId, cancellation)
                    => await IsStudentGroupExistAsync(StudentGroupId.GetValueOrDefault()))
                .When(x => x.StudentGroupId != null)
                .OnAnyFailure(x =>
                {
                    throw new EntityValidationException(ValidationConstants.StudentGroupNotValid);
                });

            RuleFor(x => x.ThemeId)
                                .MustAsync(async (ThemeId, cancellation)
                    => await IsThemeExistAsync(ThemeId.GetValueOrDefault()))
                .When(x => x.ThemeId != null)
                .OnAnyFailure(x =>
                {
                    throw new EntityValidationException(ValidationConstants.ThemeNotValid);
                });
            
            RuleFor(x => x.MentorId)
                .MustAsync(async (MentorId, cancellation)
                    => await IsMentorExistAsync(MentorId.GetValueOrDefault()))
                .When(x => x.MentorId != null)
                .OnAnyFailure(x =>
                {
                    throw new EntityValidationException(ValidationConstants.MentorNotValid);
                });

            RuleFor(x => x.EventEnd)
                .Must((x, cancellation) => x.EventStart.HasValue && x.EventEnd.HasValue 
                    && (x.EventEnd > x.EventStart || x.EventEnd.Equals(x.EventStart)))
                .When(x => x.EventEnd != null)
                .OnAnyFailure(x =>
                {
                    throw new EntityValidationException(ValidationConstants.DatesNotValid);
                });
        }

        private async Task<bool> IsMentorExistAsync(long id)
        {
            return await _unitOfWork.MentorRepository.IsEntityExistAsync(id);
        }

        private async Task<bool> IsThemeExistAsync(long id)
        {
            return await _unitOfWork.ThemeRepository.IsEntityExistAsync(id);
        }

        private async Task<bool> IsStudentGroupExistAsync(long id)
        {
            return await _unitOfWork.StudentGroupRepository.IsEntityExistAsync(id);
        }
    }
}
