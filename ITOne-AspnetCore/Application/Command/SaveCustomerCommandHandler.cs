using ITOne_AspnetCore.Repository;
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
        public IEventBus _eventBus;
        public SaveCustomerCommandHandler(ICustomerRepository repo, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _repo = repo;
        }
        public Task<Unit> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            var dd = _repo.Get().FirstOrDefault();
            _eventBus.Publish(new CustomerCreatedEvent() { Name = "Test" });
            throw new NotImplementedException();
        }
    }
}
