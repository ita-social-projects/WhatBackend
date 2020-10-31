using CharlieBackend.Business.Options;
using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.Root
{
    public class CompositionRoot
    {
        // Inject your dependencies here
        public static void InjectDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                  b => b.MigrationsAssembly("CharlieBackend.Api"));
            });

            services.AddSingleton<ICredentialsSenderService>
            (
                x => new EmailCredentialsSenderService(
                configuration.GetSection("CredentialsSendersSettings").GetSection("email").Value,
                configuration.GetSection("CredentialsSendersSettings").GetSection("password").Value)
            );

            services.Configure<AuthOptions>(configuration.GetSection("AuthOptions"));

            #region

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IMentorOfCourseRepository, MentorOfCourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();

            #endregion
        }
    }
}
