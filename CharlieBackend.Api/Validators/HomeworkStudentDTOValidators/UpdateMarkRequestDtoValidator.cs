using CharlieBackend.Core.DTO.HomeworkStudent;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Validators.HomeworkStudentDTOValidators
{
    public class UpdateMarkRequestDtoValidator : AbstractValidator<UpdateMarkRequestDto>
    {
        public UpdateMarkRequestDtoValidator()
        {
            RuleFor(x => x.StudentHomeworkId)
                .GreaterThan(0);
            RuleFor(x => x.StudentMark)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
        }
    }
}
