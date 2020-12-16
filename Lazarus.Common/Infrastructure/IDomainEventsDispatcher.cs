using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.infrastructure
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}
