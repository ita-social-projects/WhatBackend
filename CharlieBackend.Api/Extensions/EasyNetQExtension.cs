using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.Api.Extensions
{
    public static class EasyNetQExtension
    {
        public static void AddEasyNetQ(this IServiceCollection service, string rabbitmqConnectionString)
        {
            var bus = RabbitHutch.CreateBus(rabbitmqConnectionString);
            service.AddSingleton<IBus>(bus);
        }
    }
}
