using CharlieBackend.Panel.Models.EventColor;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class EventColorService : IEventColorService
    {
        private readonly IApiUtil _apiUtil;
        private readonly EventsColorsApiEndpoints _colorsApiEndpoints;

        public EventColorService(IApiUtil apiUtil, IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;

            _colorsApiEndpoints = options.Value.Urls.ApiEndpoints.Colors;
        }

        public async Task<IList<EventColorViewModel>> GetAllEventColorsAsync()
        {
            var getAllEventColorsEndpoint = _colorsApiEndpoints.GetAllColorsEndpoint;

            return await _apiUtil.GetAsync<IList<EventColorViewModel>>(getAllEventColorsEndpoint);
        }
    }
}
