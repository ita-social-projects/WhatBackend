using CharlieBackend.AdminPanel.Services;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils;
using CharlieBackend.AdminPanel.Utils.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.AdminPanel.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Adding application's custom services into container for DI.
        /// </summary>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IHttpUtil, HttpUtil>();
            services.AddScoped<IApiUtil, ApiUtil>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ICalendarService, CalendarService>();
        }
    }
}
