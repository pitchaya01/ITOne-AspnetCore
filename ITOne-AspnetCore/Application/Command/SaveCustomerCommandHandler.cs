using ITOne_AspnetCore.Domain;
using ITOne_AspnetCore.Repository;
using Lazarus.Common.Attributes;
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
        public ICustomerRepository _repoCustomer;
        public IRepositoryBase<Address> _repoAddr;
        public IEventBus _eventBus;
        public SaveCustomerCommandHandler(ICustomerRepository repo, IEventBus eventBus, IRepositoryBase<Address> addrRepo)
        {
            _repoAddr = addrRepo;
            _eventBus = eventBus;
            _repoCustomer = repo;
        }
        public async Task<Unit> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
           // var c = _repoCustomer.Get(s => s.Name == "Test").FirstOrDefault();

            //var name = _repo.GetCustomerName();
              var customer = Customer.Create("Customer");
            _repoCustomer.Add(customer);
            //c.UpdateName("Test");

            //var a = Address.AddAddr("Test", c.AggregateId);
            //_repoAddr.Add(a);

            //_repoCustomer.Update(c);
            _repoCustomer.Commit();


            //AopTest();
            //  _eventBus.Publish(new CustomerCreatedEvent() { Name = "Test" });

            return Unit.Value;
        }
        [Retry(RetryCount = 3)]
        public string AopTest()
        {
            throw new Exception("Exception");

        }
    }
}
