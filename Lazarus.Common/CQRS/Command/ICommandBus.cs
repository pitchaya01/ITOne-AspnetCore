using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.CQRS.Command
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}
