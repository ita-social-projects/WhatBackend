using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailSendingService.Services
{
	public class Subsciber
	{
		private readonly IBus _bus;

		public Subsciber(IBus bus)
		{
			_bus = bus;
		}

		public string Listen()
		{
			_bus.PubSub.Subscribe<TextMessage>("test", HandleTextMessage);
			return "Done";
		}
		

		static void HandleTextMessage(TextMessage textMessage)
		{
			Console.WriteLine("Got message: {0}", textMessage.Text);
		}
	}


	public class TextMessage
	{
		public string Text { get; set; }
	}
}
