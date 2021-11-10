using CharlieBackend.Panel.Services;
using CharlieBackend.Panel.Services.Interfaces;
using CharlieBackend.Panel.Utils;
using CharlieBackend.Panel.Utils.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace CharlieBackend.Panel.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Adding application's custom services into container for DI.
        /// </summary>
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IApiUtil, ApiUtil>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<ILessonService, LessonService>();

            services.AddHttpClient<IHttpUtil, HttpUtil>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("Urls:Api:Https").Value);
            })
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        }
    }
}
