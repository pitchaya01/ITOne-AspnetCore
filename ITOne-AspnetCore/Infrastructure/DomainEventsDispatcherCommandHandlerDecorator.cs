using ITOne_AspnetCore.Api.User.Database;
using Lazarus.Common.infrastructure;
using MediatR;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Infrastructure
{

    public class DomainEventsDispatcherCommandHandlerDecorator<T, TResponse> : IRequestHandler<T, TResponse> where T : IRequest<TResponse>
    {
        private readonly IRequestHandler<T, TResponse> _decorated;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly DbDataContext _db;
        public DomainEventsDispatcherCommandHandlerDecorator(
            IRequestHandler<T, TResponse> decorated,
            DbDataContext db,
            IDomainEventsDispatcher domainEventsDispatcher
            )
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _db = db;
            _decorated = decorated;
        }

        public async Task<TResponse> Handle(T command, CancellationToken cancellationToken)
        {
            var r = await this._decorated.Handle(command, cancellationToken);
            //_db.SaveChanges();
            await _domainEventsDispatcher.DispatchEventsAsync();

            return r;
        }
    }


    public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
    {
        private readonly INotificationHandler<T> _decorated;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<T> decorated)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _decorated = decorated;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(notification, cancellationToken);

        }
    }
}
