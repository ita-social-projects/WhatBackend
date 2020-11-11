using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EasyNetQ;
using Serilog;
using EasyNetQ.AutoSubscribe;
using System.Reflection;

namespace WhatBackend.EmailRenderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //  services.AddControllers();
            services.AddCors();
            services.AddSingleton(RabbitHutch.CreateBus("host=localhost"));
            services.AddSingleton<MessageDispatcher>();
            services.AddSingleton(provider =>
            {
                var subscriber = new AutoSubscriber(provider.GetRequiredService<IBus>(), "EmailRenderService")
                {
                    AutoSubscriberMessageDispatcher = provider.GetRequiredService<MessageDispatcher>()
                };
                return subscriber;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
            )
        {
            //registering message consumers
            app.ApplicationServices.GetRequiredService<AutoSubscriber>().SubscribeAsync(new[] { Assembly.GetExecutingAssembly() });

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
