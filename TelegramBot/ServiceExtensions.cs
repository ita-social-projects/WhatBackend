using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using TelegramBot.Models.Commands;
using TelegramBot.Services;
using TelegramBot.Services.Interfaces;
using TelegramBot.Utils;
using TelegramBot.Utils.Interfaces;

namespace TelegramBot
{
    public static class ServiceExtensions
    {
        public static void AddBotCommands(this IServiceCollection services)
        {
            services.AddScoped<StartCommand>();
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IApiUtil, ApiUtil>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            //ToDo: add IProtected service

            services.AddHttpClient<IHttpUtil, HttpUtil>(client =>
            {
#if DEBUG  
                client.BaseAddress = new Uri(configuration.GetSection("Urls:Api:Http").Value);
#else
                client.BaseAddress = new Uri(configuration.GetSection("Urls:Api:Https").Value);
#endif
            })
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        }
    }
}
