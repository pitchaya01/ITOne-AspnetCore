using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Application.Command
{
    public class SaveCustomerCommandHandler : IRequestHandler<SaveCustomerCommand>
    {
        public Task<Unit> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
