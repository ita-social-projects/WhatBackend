using EasyNetQ;
using Serilog.Context;
using System.Threading;
using EasyNetQ.AutoSubscribe;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.Entities;
using System.Net.Mail;

namespace WhatBackend.EmailSendingService.Services
{
	public class EmailSendingConsumer : IConsumeAsync<EmailData>
    {
        private readonly ILogger<EmailSendingConsumer> _logger;
        private readonly IBus _bus;
        public EmailSendingConsumer(ILogger<EmailSendingConsumer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task ConsumeAsync(EmailData data, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"EmailRender recived: {data}");

        }


    }

}

