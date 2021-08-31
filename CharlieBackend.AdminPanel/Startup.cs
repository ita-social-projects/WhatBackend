using AutoMapper;
using CharlieBackend.AdminPanel.Extensions;
using CharlieBackend.AdminPanel.Middlewares;
using CharlieBackend.AdminPanel.Models.Mapping;
using CharlieBackend.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CharlieBackend.AdminPanel
{
    public class Startup
    {
        public readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.Get<ApplicationSettings>();

            services.Configure<ApplicationSettings>(Configuration);

            services.AddHttpContextAccessor();

            services.AddServices(Configuration);

            // AutoMapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ViewModelMapping());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddCors(options =>
            {
                if (config.Cors != null && config.Cors.AllowedOrigins != null)
                {
                    options.AddPolicy("default", policy =>
                    {
                        policy.WithOrigins(config.Cors.AllowedOrigins.ToArray())
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                    });
                }
            });

            services.AddAuthorization();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                {
                     options.LoginPath = new PathString("/Account/Login");
                 });

            services.AddControllersWithViews()
                .AddJsonConverter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment("DevelopmentLocalhost")
                || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("default");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandleMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
