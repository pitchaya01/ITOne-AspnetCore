using MediatR;
using System.Text.Json.Serialization;

namespace Lazarus.Common.Application
{
    public class DomainNotificationBase<T> : IDomainEventNotification<T>, INotification
    {
        [JsonIgnore]
        public T DomainEvent { get; }

        public DomainNotificationBase(T domainEvent)
        {
            this.DomainEvent = domainEvent;
        }
    }
}
