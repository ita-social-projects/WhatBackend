using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.AdminPanel
{
    public class ApplicationSettings
    {
        public CorsSettings Cors { get; set; }

        public UrlsSettings Urls { get; set; }

        public CookiesSettings Cookies { get; set; }
    }

    public class UrlsSettings
    {
        public ApiSettings Api { get; set; }
    }

    public class ApiSettings
    {
        public string Http { get; set; }

        public string Https { get; set; }
    }

    public class CookiesSettings
    {
        public string SecureKey { get; set; }
    }

    public class CorsSettings
    {
        public CorsSettings()
        {
            AllowedOrigins = new List<string>();
        }

        public List<string> AllowedOrigins { get; set; }
    }

}
