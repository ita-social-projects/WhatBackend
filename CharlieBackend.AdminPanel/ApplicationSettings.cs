using System.Collections.Generic;

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

        public ApiEndpoints ApiEndpoints { get; set; }
    }

    public class ApiSettings
    {
        public string Http { get; set; }

        public string Https { get; set; }
    }

    public class ApiEndpoints
    {
        public CoursesApiEndpoints Courses { get; set; }
    }

    public class CoursesApiEndpoints
    {
        public string GetAllCoursesEndpoint { get; set; }
        public string AddCourseEndpoint { get; set; }
        public string UpdateCourseEndpoint { get; set; }
        public string DisableCourseEndpoint { get; set; }
        public string EnableCourseEndpoint { get; set; }
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
