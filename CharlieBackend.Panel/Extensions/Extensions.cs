﻿using CharlieBackend.Panel.Services;
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
            services.AddScoped<IBinaryResultApiUtil, ApiUtil>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<ISecretaryService, SecretaryService>();
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IClassbookService, ClassbookService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IMarkService, MarkService>();

            services.AddHttpClient<IHttpUtil, HttpUtil>(client =>
            {
#if DEBUG  
                client.BaseAddress = new Uri(configuration.GetSection("Urls:Api:Http").Value);
#else
                client.BaseAddress = new Uri(configuration.GetSection("Urls:Api:Https").Value);
#endif
            })
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        }
    }
}
