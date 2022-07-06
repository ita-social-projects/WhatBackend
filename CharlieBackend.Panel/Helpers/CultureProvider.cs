using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;
using CharlieBackend.Panel.Models.Languages;

namespace CharlieBackend.Panel.Helpers
{
    public class CultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            await Task.Yield();
            return new ProviderCultureResult(Languages.language.ToDescriptionString());
        }
    }
}
