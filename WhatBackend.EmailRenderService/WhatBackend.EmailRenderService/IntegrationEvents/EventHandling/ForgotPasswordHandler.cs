using EasyNetQ;
using Serilog.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.DTO.EmailData;
using CharlieBackend.Core.IntegrationEvents.Events;
using WhatBackend.EmailRenderService.Services.Interfaces;

namespace WhatBackend.EmailRenderService.IntegrationEvents.EventHandling
{
    public class ForgotPasswordHandler
    {
        private const string queueName = "EmailSenderService";
        private readonly ILogger<ForgotPasswordHandler> _logger;
        private readonly IBus _bus;
        private readonly IMessageTemplateService _messageTemplate;

        public ForgotPasswordHandler(
                ILogger<ForgotPasswordHandler> logger,
                IBus bus,
                IMessageTemplateService messageTemplate
                )
        {
            _logger = logger;
            _bus = bus;
            _messageTemplate = messageTemplate;
        }

        public async Task HandleAsync(ForgotPasswordEvent message)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{message}"))
            {
                _logger.LogInformation($"Registration success: {message}");

                _logger.LogInformation("-----Publishing ForgotPasswordEvent integration event----- ");

                await _bus.SendReceive.SendAsync(queueName, new EmailData
                {
                    RecipientMail = message.RecepientMail,
                    EmailBody = _messageTemplate.GetRegistrationSuccessTemplate(message)
                });
            }
        }
    }
}
