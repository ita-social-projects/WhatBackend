using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;

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
