using Lazarus.Common.DAL;
using Lazarus.Common.EventMessaging;
using Shared.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Domain.Event
{
    public class CustomerCreatedEventHandler : IIntegrationEventHandler<CustomerCreatedEvent>
    {
        public IRepositoryBase<Customer> _repo;
        public CustomerCreatedEventHandler(IRepositoryBase<Customer> repo)
        {
            _repo = repo;
        }
        public Task Handle(CustomerCreatedEvent @event)
        {
            throw new NotImplementedException();
        }

        public Task Validate(CustomerCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
