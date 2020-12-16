using Autofac;
using RdKafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.DI
{
    public static class DomainEvents
    {
        public static ILifetimeScope _Container;
        public static EventConsumer _Consumer; 
        public static void SetContainer(IContainer container)
        {
            _Container = container;

        }
    }
}
