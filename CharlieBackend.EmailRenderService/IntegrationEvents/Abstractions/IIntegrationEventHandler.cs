using System.Threading.Tasks;

namespace CharlieBackend.EmailRenderService.IntegrationEvents.Abstractions
{
    public interface IIntegrationEventHandler
    {
        public interface IIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IEvent
        {
            Task HandleAsync(TIntegrationEvent @event);
        }
    }
}
