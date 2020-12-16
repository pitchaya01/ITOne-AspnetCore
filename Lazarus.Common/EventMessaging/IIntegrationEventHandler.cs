using System.Threading.Tasks;

namespace Lazarus.Common.EventMessaging
{

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Validate(TIntegrationEvent @event);
        Task Handle(TIntegrationEvent @event);
    }
}