using Autofac;
using Lazarus.Common.CQRS.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazarus.Common.DI;

namespace Lazarus.Common.CQRS.Implement
{
    public class CustomDependencyResolver : ICustomDependencyResolver
    {
        private ILifetimeScope Container
        {
            get { return DomainEvents._Container; }
        }

        public CustomDependencyResolver()
        {

        }

        public TResolved Resolve<TResolved>() => Container.Resolve<TResolved>();
    }
}
