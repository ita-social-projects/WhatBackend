using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
