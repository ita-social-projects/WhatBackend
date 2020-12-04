using EasyNetQ;
using Serilog.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.DTO.EmailData;
using CharlieBackend.Core.IntegrationEvents.Events;
using WhatBackend.EmailRenderService.Services.Interfaces;

namespace WhatBackend.EmailRenderService.IntegrationEvents.EventHandling
{
    public class AccountApprovedHanlder
    {
        private const string queueName = "EmailSenderService";
        private readonly ILogger<AccountApprovedHanlder> _logger;
        private readonly IBus _bus;
        private readonly IMessageTemplateService _messageTemplate;

        public AccountApprovedHanlder(
                ILogger<AccountApprovedHanlder> logger,
                IBus bus,
                IMessageTemplateService messageTemplate
                )
        {
            _logger = logger;
            _bus = bus;
            _messageTemplate = messageTemplate;
        }

        public async Task HandleAsync(AccountApprovedEvent message)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{message}"))
            {
                _logger.LogInformation($"Account has been approved: {message}");

                _logger.LogInformation("-----Publishing AccountApprovedEvent integration event----- ");

                await _bus.SendReceive.SendAsync(queueName, new EmailData
                {
                    RecipientMail = message.RecepientMail,
                    EmailBody = _messageTemplate.GetAccountApprovedTemplate(message)
                });
            }
        }
    }
}
