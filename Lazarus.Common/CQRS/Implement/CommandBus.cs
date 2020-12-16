using Lazarus.Common.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.CQRS.Implement
{
    public class CommandBus : ICommandBus
    {
    
        ICommandHandlerExecutor CommandHandlerExecutor { get; }

        public CommandBus(ICommandHandlerExecutor commandHandlerExecutor)
        {
            CommandHandlerExecutor = commandHandlerExecutor;

     
        }

        public void Send<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var str = nameof(CommandBus);
   
            ProccessBus(command);
        }


        void ProccessBus(ICommand command)
        {


            var commandType = command.GetType();
            var executorType = CommandHandlerExecutor.GetType();

            executorType.GetMethod(nameof(ICommandHandlerExecutor.Execute))
                .MakeGenericMethod(commandType)
                .Invoke(CommandHandlerExecutor, new[] { command });
        }

        Task ICommandBus.SendAsync<TCommand>(TCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
