using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.Api.Extensions
{
    /// <summary>
    /// Class for injection EasyNetQ
    /// </summary>
    public static class EasyNetQExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public static void AddEasyNetQ(this IServiceCollection service, string rabbitmqConnectionString)
        {
            var bus = RabbitHutch.CreateBus(rabbitmqConnectionString);
            service.AddSingleton<IBus>(bus);
        }
    }
}
