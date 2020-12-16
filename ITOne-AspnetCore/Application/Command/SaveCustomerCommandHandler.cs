using ITOne_AspnetCore.Domain;
using ITOne_AspnetCore.Repository;
using Lazarus.Common.DAL;
using Lazarus.Common.EventMessaging;
using MediatR;
using Shared.Event;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Application.Command
{
    public class SaveCustomerCommandHandler : IRequestHandler<SaveCustomerCommand>
    {
        public ICustomerRepository _repo;
        public IRepositoryBase<Address> _repoAddr;
        public IEventBus _eventBus;
        public SaveCustomerCommandHandler(ICustomerRepository repo, IEventBus eventBus, IRepositoryBase<Address> addrRepo)
        {
            _repoAddr = addrRepo;
            _eventBus = eventBus;
            _repo = repo;
        }
        public async Task<Unit> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            var c = _repo.Get(s=>s.Name=="Test").FirstOrDefault();
            //  var customer = Customer.Create("Customer");
            //_repo.Add(customer);
            c.UpdateName("Test");
       
            var a = Address.AddAddr("Test", c.AggregateId);
            _repoAddr.Add(a) ;
            
            _repo.Update(c);
            _repo.Commit();

          //  _eventBus.Publish(new CustomerCreatedEvent() { Name = "Test" });

            return Unit.Value;
        }
    }
}
