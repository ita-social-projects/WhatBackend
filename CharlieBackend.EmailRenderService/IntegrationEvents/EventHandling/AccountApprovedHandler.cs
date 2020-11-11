using CharlieBackend.EmailRenderService.IntegrationEvents.Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static CharlieBackend.EmailRenderService.IntegrationEvents.Abstractions.IIntegrationEventHandler;
using Serilog.Context;
using EasyNetQ;

namespace CharlieBackend.EmailRenderService.IntegrationEvents.EventHandling
{
    public class AccountApprovedHandler : IIntegrationEventHandler<AccountApprovedEvent>
    {
        #region
        private readonly ILogger<AccountApprovedHandler> _logger;
        private readonly IBus _bus;
        #endregion

        public AccountApprovedHandler(ILogger<AccountApprovedHandler> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task HandleAsync(AccountApprovedEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event}"))
            {

                _logger.LogInformation($"Account has been approved: {@event}");

                string header = "WHAT Backend credentials";
                string emailBody = 
                        "<table style='width: 100% !important;border-collapse: collapse; width: 100% !important;height: 100%;background: #efefef;-webkit-font-smoothing: antialiased;-webkit-text-size-adjust: none;'>" +
                        "<tr>" +
                            "<td style='display: block !important;clear: both !important;margin: 0 auto !important;max-width: 580px !important;'>" +
                            "<table style='width: 100% !important;border-collapse: collapse;'>" +
                                "<tr>" +
                                    "<td style='padding: 45px 0; position:relative; background-repeat: no-repeat; background-size: cover;background-image: url(https://cdnssinc-dev.azureedge.net/img/home/gradient-map-1.jpg);' align='center'>" +
                                          "<h1 style='font-size: 32px; margin-bottom: 20px;line-height: 1.25;'>" + header + "</h1>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='display: block !important;clear: both !important;margin: 0 auto !important;max-width: 580px !important;'>" +
                                        "<p>" +
                                            "Welcome, " + @event.FirstName + " " + @event.LastName + "! " + "Your account has been successfully approved!" +
                                        "</p>" +
                                       "<p>" +
                                            "<ul>" +
                                                "<li>" +
                                                    "Your role: " + @event.Role + 
                                                "</li>" +
                                            "</ul>" +
                                        "</p>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td class='container'>" +
                            "<table style='width: 100% !important;border-collapse: collapse;'>" +
                                "<tr>" +
                                    "<td class='content footer' align='center'>" +
                                        "<p>© Copyright 2020 SoftServe</p>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                        "</td>" +
                    "</tr>" +
                "</table>";

                (string, string) data = (@event.RecepientMail, emailBody);

                _logger.LogInformation("-----Publishing AccountApprovedEvent integration event----- ");

                await _bus.PubSub.PublishAsync(data, "EmailSenderService").ConfigureAwait(false);

                await Task.CompletedTask;
            }
        }
    }
}
