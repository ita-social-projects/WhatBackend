using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;
using CharlieBackend.Panel.Models.Languages;
using System.Linq;

namespace CharlieBackend.Panel.Helpers
{
    public class CultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            await Task.Yield();

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return new ProviderCultureResult(Languages.language.ToDescriptionString());
            }

            return new ProviderCultureResult(httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimsConstants.Localization).Value);
        }
    }
}
