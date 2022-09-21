//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Telegram.Bot;
//using Telegram.Bot.Types.Enums;

//namespace TelegramBot.Services
//{
//    public class ConfigureWebhook : IHostedService
//    {
//        private readonly ILogger<ConfigureWebhook> _logger;
//        private readonly IServiceProvider _services;
//        private readonly BotSettings _botConfig;

//        public ConfigureWebhook(ILogger<ConfigureWebhook> logger,
//                                IServiceProvider serviceProvider,
//                                IConfiguration configuration)
//        {
//            _logger = logger;
//            _services = serviceProvider;
//            _botConfig = configuration.GetSection("BotSettings").Get<BotSettings>();
//        }

//        public async Task StartAsync(CancellationToken cancellationToken)
//        {
//            using var scope = _services.CreateScope();
//            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

//            var webhookAddress = @$"{_botConfig.Url}/bot/{_botConfig.Key}";
//            _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
//            await botClient.SetWebhookAsync(
//                url: webhookAddress,
//                allowedUpdates: Array.Empty<UpdateType>(),
//                cancellationToken: cancellationToken);
//        }

//        public async Task StopAsync(CancellationToken cancellationToken)
//        {
//            using var scope = _services.CreateScope();
//            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

//            _logger.LogInformation("Removing webhook");
//            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
//        }
//    }
//}


using Microsoft.Extensions.Configuration;
using System;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class ConfigurationBot
    {
        public static void Configure(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            BotSettings.Url = configuration.GetSection("BotSettings")
                .GetSection("Url").Value;
            BotSettings.Key = configuration.GetSection("BotSettings")
                .GetSection("Key").Value;
            BotSettings.Name = configuration.GetSection("BotSettings")
                .GetSection("Name").Value;

            Bot.Get(serviceProvider).Wait();
        }
    }
}