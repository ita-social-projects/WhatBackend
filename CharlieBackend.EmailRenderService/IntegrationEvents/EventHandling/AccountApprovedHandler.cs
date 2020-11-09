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


                //probably some logic here


                _logger.LogInformation("-----Publishing AccountApprovedEvent integration event----- ");

                _bus.PubSub.Publish(@event);

                await Task.CompletedTask;
            }
        }
    }
}
