using Lazarus.Common.CQRS.Command;
using Lazarus.Common.EventMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.CQRS.Implement
{
    public class CommandHandlerExecutor : ICommandHandlerExecutor
    {
        ICustomDependencyResolver CustomDependencyResolver { get; }

        public CommandHandlerExecutor(ICustomDependencyResolver customDependencyResolver)
        {
            CustomDependencyResolver = customDependencyResolver;
        }

        public async Task Execute<TCommand>(TCommand command) where TCommand : IntegrationEvent
            => await CustomDependencyResolver.Resolve<IIntegrationEventHandler<TCommand>>().Handle(command);
    }
}
