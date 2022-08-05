using AutoMapper;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Panel.Extensions;
using CharlieBackend.Panel.Helpers;
using CharlieBackend.Panel.Models.Languages;
using CharlieBackend.Panel.Middlewares;
using CharlieBackend.Panel.Models.Mapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

namespace CharlieBackend.Panel
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
                .AddJsonSerializer();

            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo(Language.En.ToDescriptionString()),
                    new CultureInfo(Language.Uk.ToDescriptionString())
                };
                options.DefaultRequestCulture = new RequestCulture(culture: Language.En.ToDescriptionString(), uiCulture: Language.En.ToDescriptionString());

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new CultureProvider());
            });
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

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

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
