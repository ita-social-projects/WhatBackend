using Serilog;
using EasyNetQ;
using System.Reflection;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatBackend.EmailSendingService.Services;
using WhatBackend.EmailSendingService.Services.Interfaces;

namespace WhatBackend.EmailSendingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSingleton(RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ")));

            services.AddSingleton<MessageDispatcher>();

            services.AddSingleton<IEmailSender>(
                x => new EmailSender(
                    Configuration.GetSection("SendersSettings").GetSection("email").Value,
                    Configuration.GetSection("SendersSettings").GetSection("password").Value)
                );

            services.AddScoped<EmailSendingConsumer>();

            services.AddSingleton(provider =>
            {
                var subscriber = new AutoSubscriber(provider.GetRequiredService<IBus>(), "EmailSenderService")
                {
                    AutoSubscriberMessageDispatcher = provider.GetRequiredService<MessageDispatcher>(),
                    ConfigureSubscriptionConfiguration = new System.
                        Action<ISubscriptionConfiguration>(c => c.WithQueueName("EmailSenderService"))
                };
                
                return subscriber;
            });
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //registering message consumers
            app.ApplicationServices.GetRequiredService<AutoSubscriber>()
                    .SubscribeAsync(new[] { Assembly.GetExecutingAssembly() });

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
