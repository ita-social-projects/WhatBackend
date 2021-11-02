using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace CharlieBackend.Api.SwaggerExamples.ThemesController
{
    class GetAllThemesResponse : IExamplesProvider<IList<ThemeDto>>
    {
        public IList<ThemeDto> GetExamples()
        {
            return new List<ThemeDto>()
            {
                new ThemeDto
                {
                    Id = 123,
                    Name = "Picking up of flowers"
                },
                new ThemeDto
                {
                    Id = 133,
                    Name = "Drying of flowers"
                },
            };
        }
    }
}
