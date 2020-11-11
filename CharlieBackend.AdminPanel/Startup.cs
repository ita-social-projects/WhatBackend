using System;
using AutoMapper;
using CharlieBackend.AdminPanel.Models.Mapping;
using CharlieBackend.AdminPanel.Services;
using CharlieBackend.AdminPanel.Services.Interfaces;
using CharlieBackend.AdminPanel.Utils;
using CharlieBackend.AdminPanel.Utils.Interfaces;
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


            services.AddTransient<IHttpUtil, HttpUtil>();
            services.AddTransient<IApiUtil, ApiUtil>();

            services.AddTransient<IStudentService, StudentService>(); 
            services.AddTransient<IStudentGroupService, StudentGroupService>();

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
                     options.LoginPath = new PathString("/admin/account/LogIn");
                 });

            services.AddControllersWithViews();
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

         
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
