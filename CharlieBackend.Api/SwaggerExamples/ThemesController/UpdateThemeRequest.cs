using CharlieBackend.Core.DTO.Theme;
using Swashbuckle.AspNetCore.Filters;

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
