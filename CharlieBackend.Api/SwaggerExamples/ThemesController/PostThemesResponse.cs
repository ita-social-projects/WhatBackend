using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;

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
