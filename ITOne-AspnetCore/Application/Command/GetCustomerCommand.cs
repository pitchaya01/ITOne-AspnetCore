using ITOne_AspnetCore.Api.User.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Application.Command
{
    public class GetCustomerCommand:IRequest<string>
    {
    }
    public class GetCustomerCommandHandler : IRequestHandler<GetCustomerCommand, string>
    {
        public DbDataContext _db;
        public GetCustomerCommandHandler(DbDataContext db)
        {
            _db = db;
        }
        public async Task<string> Handle(GetCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = _db.Customers.Where(a=>a.Name.Contains("aa")).FirstOrDefault();
            return "";
        }
    }
}
