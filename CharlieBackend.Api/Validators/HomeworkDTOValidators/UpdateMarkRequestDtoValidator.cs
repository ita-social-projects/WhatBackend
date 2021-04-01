using CharlieBackend.Core.DTO.Homework;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Validators.HomeworkDTOValidators
{
    public class UpdateMarkRequestDtoValidator : AbstractValidator<UpdateMarkRequestDto>
    {
        public UpdateMarkRequestDtoValidator()
        {
            RuleFor(x => x.StudentHomeworkId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.StudentMark)
                .NotNull()
                .GreaterThan(0)
                .LessThan(100);
        }
    }
}
