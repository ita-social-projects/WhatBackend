using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;
using CharlieBackend.Panel.Models.Languages;
using System.Linq;
using System.Security.Claims;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Services;

namespace CharlieBackend.Panel.Helpers
{
    public class CultureProvider : RequestCultureProvider
    {
        private readonly ICurrentUserService _currentUserService = new CurrentUserService(new HttpContextAccessor());

        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            await Task.Yield();

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return new ProviderCultureResult(Language.En.ToDescriptionString());
            }
            return new ProviderCultureResult(_currentUserService.Localization);

        }
    }
}
