using CharlieBackend.Core.DTO;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.EventColorController
{
    public class GetEventColorByIdResponse : IExamplesProvider<EventColorDTO>
    {
        public EventColorDTO GetExamples()
        {
            return new EventColorDTO
            {
                Id = 1,
                Color = "#ff0000"
            };
        }
    }
}

