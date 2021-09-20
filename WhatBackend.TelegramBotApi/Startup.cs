using AutoMapper;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using CharlieBackend.Business.Models;
using CharlieBackend.Business.Models.Commands;
using CharlieBackend.Root;
using WhatBackend.TelegramBotApi.Extensions;

namespace WhatBackend.TelegramBotApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            CompositionRoot.InjectDependencies(services, Configuration);

            services.AddScoped<StartCommand>();
            services.AddScoped<HelloCommand>();
            services.AddScoped<GetMarkCommand>();
            services.AddScoped<StudentGroupsCommand>();
            services.AddScoped<CoursesCommand>();
            services.AddScoped<PersonalInfoCommand>();

            services.AddCors();

            services.AddEasyNetQ(Configuration
                .GetConnectionString("RabbitMQ"));

            services.AddAzureStorageBlobs(Configuration
                .GetConnectionString("AzureBlobsAccessKey"));

            services.AddHttpContextAccessor();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelMappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            ApplicationContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            var url = Configuration.GetSection("BotSettings")
                .GetSection("Url").Value;
            var key = Configuration.GetSection("BotSettings")
                .GetSection("Key").Value;
            var name = Configuration.GetSection("BotSettings")
                .GetSection("Name").Value;
            AppSettings.GetData(url, key, name);

            Bot.Get(serviceProvider).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
