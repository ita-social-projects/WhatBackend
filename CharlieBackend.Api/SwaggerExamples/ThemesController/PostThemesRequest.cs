using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.ThemesController
{
    class PostThemesRequest : IExamplesProvider<CreateThemeDto>
    {
        public CreateThemeDto GetExamples()
        {
            return new CreateThemeDto()
            {
                Name = "Picking up of flowers"
            };
        }
    }
}
