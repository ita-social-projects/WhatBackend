using EasyNetQ;
using System.Net;
using Serilog.Context;
using System.Threading;
using EasyNetQ.AutoSubscribe;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.Entities;
using System.Net.Mail;
using WhatBackend.EmailSendingService.Services.Interfaces;

namespace WhatBackend.EmailSendingService.Services
{
	public class EmailSendingConsumer : IConsumeAsync<EmailData>
    {
        private readonly ILogger<EmailSendingConsumer> _logger;
        private readonly IBus _bus;
        private readonly IEmailSender _sender;

        public EmailSendingConsumer(
                ILogger<EmailSendingConsumer> logger, 
                IBus bus,
                IEmailSender sender)
        {
            _logger = logger;
            _bus = bus;
            _sender = sender;
        }

        public async Task ConsumeAsync(EmailData data, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"EmailRender recived: {data}");

            await _sender.SendMessageAsync(data);
        }


    }

}

