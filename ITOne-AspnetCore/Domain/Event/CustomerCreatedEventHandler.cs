using ITOne_AspnetCore.Repository;
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
        public ICustomerRepository _repo;
        public CustomerCreatedEventHandler(ICustomerRepository repo)
        {
            _repo = repo;
        }
        public async Task Handle(CustomerCreatedEvent @event)
        {
            var aa = _repo.Get(a => a.Name == "Test").FirstOrDefault();
        }

        public Task Validate(CustomerCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
