using Serilog;
using EasyNetQ;
using System.Reflection;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatBackend.EmailRenderService.IntegrationEvents;
using WhatBackend.EmailRenderService.IntegrationEvents.EventHandling;

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
            services.AddSingleton(RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ")));
            services.AddSingleton<MessageDispatcher>();
            services.AddScoped<AccountApprovedConsumer>();
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

            app.UseRouting();
        }
    }
}
