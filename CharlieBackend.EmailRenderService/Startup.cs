using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EasyNetQ;
using Serilog;
using RabbitMQ;
using CharlieBackend.EmailRenderService.IntegrationEvents.Events;
using CharlieBackend.EmailRenderService.IntegrationEvents.EventHandling;
using CharlieBackend.EmailRenderService.IntegrationEvents.Abstractions;

namespace CharlieBackend.EmailRenderService
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

            
            services.AddSingleton(services => {
                
                var bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("RabbitMQ"));
                var emailRender = services.GetService<AccountApprovedHandler>();
                bus.PubSub.Subscribe<AccountApprovedEvent>("EmailRenderService", emailRender.HandleAsync);
                return bus;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBus bus)
        {
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

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
