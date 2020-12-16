using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Infrastructure.Processing
{
    public interface ICommandsDispatcher
    {
        Task DispatchCommandAsync(Guid id);
    }
    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly IMediator _mediator;

        public CommandsDispatcher(
            IMediator mediator)
        {
            this._mediator = mediator;
        
        }

        public  Task DispatchCommandAsync(Guid id)
        {

            return null;

           
        }
    }
}
