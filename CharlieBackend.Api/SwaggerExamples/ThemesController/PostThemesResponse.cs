using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.ThemesController
{
    class PostThemesResponse : IExamplesProvider<ThemeDto>
    {
        public ThemeDto GetExamples()
        {
            return new ThemeDto()
            {
                Id = 122,
                Name = "Picking up of flowers"
            };
        }
    }
}
