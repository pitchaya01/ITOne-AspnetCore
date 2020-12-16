using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.CQRS.Command
{
    public interface ICustomDependencyResolver
    {
        TResolved Resolve<TResolved>();
    }
}
