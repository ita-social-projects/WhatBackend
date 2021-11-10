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
using Hangfire;
using System.Data.Common;
using CharlieBackend.Business.Services.Notification;

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

            CompositionRoot.Configure(serviceProvider, Configuration);

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
