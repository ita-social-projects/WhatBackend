using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Api.SwaggerExamples.ThemesController
{
    class UpdateThemeRequest : IExamplesProvider<UpdateThemeDto>
    {
        public UpdateThemeDto GetExamples()
        {
            return new UpdateThemeDto()
            {
                Name = "Picking up of flowers"
            };
        }
    }
}
