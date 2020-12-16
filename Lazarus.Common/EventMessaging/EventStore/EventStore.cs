using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Lazarus.Common.Nexus.Database;
using Lazarus.Common.Utilities;

namespace Lazarus.Common.EventMessaging.EventStore
{
    public class EventStore : IEventStore
    {
        public NexusDataContext _db;

        public EventStore(NexusDataContext db)
        {
            _db = db;
        }

        public void Commit(string id,string MachinesName)
        {
            var l = _db.LogEventStores.FirstOrDefault(s => s.AggregateId == id);
            if (l == null) return;
            l.Status = "Success";
            l.Message = MachinesName;
            l.CommitDateTime = DateTime.Now;
            _db.SaveChanges();
        }

        public void CommitedFail(string id, string msg)
        {
            var l = _db.LogEventStores.FirstOrDefault(s => s.AggregateId == id);
            if (l == null) return;
            l.CommitDateTime = DateTime.Now;
            l.Status = "Error";
            l.Message = msg;
            _db.SaveChanges();
            
        }

        public bool IsCommited(string aggId)
        {
            var l = _db.LogEventStores.FirstOrDefault(s => s.AggregateId == aggId);
            if (l == null) return false;

            return l.Status=="Success";
        }

        public void Persist<TAggregate>(TAggregate aggregate) where TAggregate : IntegrationEvent
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                var env = "Dev";
                var date = DateTime.Now;
                var val = aggregate.ToJSON();
                var l = new LogEventStore();
                l.AggregateId = aggregate.AggregateId;
                l.CreateDate = DateTime.Now;
                l.EventName = env + aggregate.GetType().Name;
                l.Val = val;
                _db.LogEventStores.Add(l);
                _db.SaveChanges();
            }
        }
    }
}
