using CharlieBackend.Panel.Models.Mark;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Services
{
    public class MarkService : IMarkService
    {
        private readonly IApiUtil _apiUtil;
        private readonly MarksApiEndpoints _marksApiEndpoints;

        public MarkService(IApiUtil apiUtil, IOptions<ApplicationSettings> options)
        {
            _apiUtil = apiUtil;

            _marksApiEndpoints = options.Value.Urls.ApiEndpoints.Marks;
        }

        public async Task<MarkViewModel> GetMarkByIdAsync(long id)
        {
            var getMarkEndpoint = string.Format(_marksApiEndpoints.GetMarkEndpoint, id);
            var mark = await _apiUtil.GetAsync<MarkViewModel>(getMarkEndpoint);

            return mark;
        }
    }
}
