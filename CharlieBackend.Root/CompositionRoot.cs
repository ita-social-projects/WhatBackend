using CharlieBackend.Business.Helpers;
using CharlieBackend.Business.Models;
using CharlieBackend.Business.Options;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.FileServices.ExportFileServices;
using CharlieBackend.Business.Services.FileServices.ImportFileServices;
using CharlieBackend.Business.Services.FileServices.ImportFileServices.ImportOperators;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Business.Services.Notification;
using CharlieBackend.Business.Services.Notification.Interfaces;
using CharlieBackend.Business.Services.ScheduleServiceFolder;
using CharlieBackend.Business.Services.ScheduleServiceFolder.Helpers;
using CharlieBackend.Data;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using CharlieBackend.Root.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CharlieBackend.Root
{
    public class CompositionRoot
    {
        public static void Configure(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var url = configuration.GetSection("BotSettings")
                .GetSection("Url").Value;
            var key = configuration.GetSection("BotSettings")
                .GetSection("Key").Value;
            var name = configuration.GetSection("BotSettings")
                .GetSection("Name").Value;
            AppSettings.GetData(url, key, name);

            Bot.Get(serviceProvider).Wait();
        }
        // Inject your dependencies here
        public static void InjectDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                  builder =>
                  {
                      builder.MigrationsAssembly("CharlieBackend.Api");
                  });
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.Configure<AuthOptions>(configuration.GetSection("AuthOptions"));

            #region

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IHangfireNotificationService, HangfireNotificationService>();
            services.AddScoped<IHangfireJobService, HangfireJobService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IMentorOfCourseRepository, MentorOfCourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
            services.AddScoped<IScheduledEventRepository, ScheduledEventRepository>();
            services.AddScoped<IHomeworkRepository, HomeworkRepository>();
            services.AddScoped<IHomeworkStudentRepository, HomeworkStudentRepositrory>();
            services.AddScoped<IJobMappingRepository, JobMappingRepository>();
            services.AddScoped<ISecretaryService, SecretaryService>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ExportServiceXlsx>();
            services.AddScoped<ExportServiceCsv>();
            services.AddScoped<IExportServiceProvider, ExportServiceProvider>();
            services.AddScoped<ServiceOperatorXlsx>();
            services.AddScoped<ServiceOperatorCsv>();
            services.AddScoped<IOperatorImportProvider, ImportOperatorProvider>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IServiseImport, ServiceImport>();
            services.AddScoped<IScheduledEventHandlerFactory, ScheduledEventHandlerFactory>();
            services.AddScoped<IHomeworkStudentService, HomeworkStudentService>();
            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<ISchedulesEventsDbEntityVerifier, SchedulesEventsDbEntityVerifier>();

            services.AddBotCommands();

            #endregion
        }
    }
}
