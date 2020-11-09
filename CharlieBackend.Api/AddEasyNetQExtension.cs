using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CharlieBackend.Api
{
    public static class AddEasyNetQExtension
    {
        public static void AddEasyNetQ(this IServiceCollection service, string rabbitmqConnectionString)
        {
            var bus = RabbitHutch.CreateBus(rabbitmqConnectionString);
            service.AddSingleton<IBus>(bus);
        }

        //public static void UseEasyNetQ(this IApplicationBuilder app)
        //{
        //    var bus = app.ApplicationServices.GetRequiredService<IBus>();
        //    var autoSubscriber = new AutoSubscriber(bus, "productor")
        //    {
        //        AutoSubscriberMessageDispatcher = app.ApplicationServices.GetRequiredService<IAutoSubscriberMessageDispatcher>()
        //    };
        //    autoSubscriber.Subscribe(Assembly.GetExecutingAssembly());
        //    autoSubscriber.SubscribeAsync(Assembly.GetExecutingAssembly());
        //}
    }
}
