using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Models.Commands;

namespace TelegramBot
{
    public static class ServiceExtensions
    {
        public static void AddBotCommands(this IServiceCollection services)
        {
            services.AddScoped<StartCommand>();
        }
    }
}
