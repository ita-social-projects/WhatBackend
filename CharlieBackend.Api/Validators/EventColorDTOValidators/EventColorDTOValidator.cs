using CharlieBackend.Core.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Validators
{
    public class EventColorDTOValidator: AbstractValidator<EventColorDTO>
    {
        public EventColorDTOValidator()
        {
            RuleFor(x => x.Id)
                 .NotEmpty()
                 .NotNull()
                 .GreaterThan(0);
            RuleFor(x => x.Color)
                .NotEmpty()
                .NotNull();
        }
    }
}
