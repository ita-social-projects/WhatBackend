using CharlieBackend.Core.DTO;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.EventColorController
{
    internal class GetAllColorsResponse : IExamplesProvider<IList<EventColorDTO>>
    {
        public IList<EventColorDTO> GetExamples()
        {
            return new List<EventColorDTO>
            {
                new EventColorDTO()
                    {
                       Id = 1,
                       Color = "#ff0000"
                    },
                    new EventColorDTO()
                    {
                        Id = 2,
                        Color = "#000000"
                    }
            };
        }
    }
}
