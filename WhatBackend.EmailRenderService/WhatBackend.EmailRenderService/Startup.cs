using Serilog;
using EasyNetQ;
using System.Reflection;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using WhatBackend.EmailRenderService.Services;
using Microsoft.Extensions.DependencyInjection;
using WhatBackend.EmailRenderService.IntegrationEvents;
using WhatBackend.EmailRenderService.Services.Interfaces;
using WhatBackend.EmailRenderService.IntegrationEvents.EventHandling;
using CharlieBackend.Core.IntegrationEvents.Events;
using System;

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
            
            services.AddCors();
            services.AddSingleton<IMessageTemplateService, MessageTemplateService>();
            services.AddSingleton(RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ")));
            services.AddSingleton<MessageDispatcher>();
            services.AddScoped<EventConsumer>();

            services.AddSingleton(provider =>
            {
                var subscriber = new AutoSubscriber(provider.GetRequiredService<IBus>(), "EmailRenderService")
                {
                    AutoSubscriberMessageDispatcher = provider.GetRequiredService<MessageDispatcher>(),
                    ConfigureSubscriptionConfiguration = new System.Action<ISubscriptionConfiguration>(c => c.WithQueueName("EmailRenderService"))
                };

                return subscriber;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime,
            IBus bus
            )
        {
            //When the application is stopping we need to make unsubscribe for IBus.
            //Important! Even in debugg mode finish this application with Ctrl+C in a console to make sure
            //what unsubscribe was done well.
            applicationLifetime.ApplicationStopping.Register(OnShutdown, bus);

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

            app.UseRouting();
        }

        private void OnShutdown(object toDispose)
        {
            ((IDisposable)toDispose).Dispose();
        }
    }
}
