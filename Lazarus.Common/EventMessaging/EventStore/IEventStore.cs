using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.EventMessaging.EventStore
{
    public interface IEventStore
    {
        void Persist<TAggregate>(TAggregate aggregate) where TAggregate : IntegrationEvent;
        bool IsCommited(string aggId);
        void Commit(string id, string MachinesName);
        void CommitedFail(string id, string msg);
        //TAggregate GetById<TAggregate, TEvent>(Guid id) where TAggregate : IAggregateRoot, new() where TEvent : class, IEvent;

    }
}
