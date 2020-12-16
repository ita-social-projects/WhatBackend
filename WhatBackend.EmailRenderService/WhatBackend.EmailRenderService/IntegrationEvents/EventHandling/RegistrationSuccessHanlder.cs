using EasyNetQ;
using Serilog.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CharlieBackend.Core.DTO.EmailData;
using CharlieBackend.Core.IntegrationEvents.Events;
using WhatBackend.EmailRenderService.Services.Interfaces;

namespace WhatBackend.EmailRenderService.IntegrationEvents.EventHandling
{
    public class RegistrationSuccessHanlder
    {
        private const string queueName = "EmailSenderService";
        private readonly ILogger<RegistrationSuccessHanlder> _logger;
        private readonly IBus _bus;
        private readonly IMessageTemplateService _messageTemplate;

        public RegistrationSuccessHanlder(
                ILogger<RegistrationSuccessHanlder> logger,
                IBus bus,
                IMessageTemplateService messageTemplate
                )
        {
            _logger = logger;
            _bus = bus;
            _messageTemplate = messageTemplate;
        }

        public async Task HandleAsync(RegistrationSuccessEvent message)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{message}"))
            {
                _logger.LogInformation($"Registration success: {message}");

                _logger.LogInformation("-----Publishing RegistrationSuccesEvent integration event----- ");

                await _bus.SendReceive.SendAsync(queueName, new EmailData
                {
                    RecipientMail = message.RecepientMail,
                    EmailBody = _messageTemplate.GetEmailNotifyTemplate(string.Format("Welcome, {0} {1}!" +
                            "Your account registration is success! Please await approving for your account.",
                            message.FirstName, message.LastName))
                });
            }
        }
    }
}
