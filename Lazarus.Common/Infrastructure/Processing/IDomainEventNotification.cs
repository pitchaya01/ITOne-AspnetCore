namespace Lazarus.Common.infrastructure.Processing
{
    public interface IDomainEventNotification<out TEventType>
    {
        TEventType DomainEvent { get; }
    }
}