//using Autofac;
//using Lazarus.Common.infrastructure;
//using Lazarus.Common.infrastructure.Processing;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac.Core;
//using Autofac;
//using Autofac.Core;
//using Autofac.Features.Variance;
//using Autofac.Integration.WebApi;
//using Lazarus.Common.Nexus.Database;
//using Lazarus.Common.CQRS.Implement;
//using Lazarus.Common.EventMessaging.EventStore;
//using Lazarus.Common.EventMessaging.Implement;
//using Lazarus.Common.EventMessaging;
//using System.Reflection;
//using MediatR.Pipeline;

//namespace Lazarus.Common.Infrastructure.Processing
//{
//    public class MediatorModule : Autofac.Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.RegisterSource(new ScopedContravariantRegistrationSource(
//                typeof(IRequestHandler<,>),
//                typeof(INotificationHandler<>),
//                typeof(IValidator<>)
//            ));

//            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

//            var mediatrOpenTypes = new[]
//            {
//            typeof(IRequestHandler<,>),
//            typeof(INotificationHandler<>),
//            typeof(IValidator<>),
//        };

//            foreach (var mediatrOpenType in mediatrOpenTypes)
//            {
//                builder
//                    .RegisterAssemblyTypes(typeof(GetCustomerOrdersQuery).GetTypeInfo().Assembly)
//                    .AsClosedTypesOf(mediatrOpenType)
//                    .AsImplementedInterfaces();
//            }

//            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
//            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

//            builder.Register<ServiceFactory>(ctx =>
//            {
//                var c = ctx.Resolve<IComponentContext>();
//                return t => c.Resolve(t);
//            });

//        }

//        public class ScopedContravariantRegistrationSource : IRegistrationSource
//        {
//            private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
//            private readonly List<Type> _types = new List<Type>();

//            public ScopedContravariantRegistrationSource(params Type[] types)
//            {
//                if (types == null)
//                    throw new ArgumentNullException(nameof(types));
//                if (!types.All(x => x.IsGenericTypeDefinition))
//                    throw new ArgumentException("Supplied types should be generic type definitions");
//                _types.AddRange(types);
//            }

//            public IEnumerable<IComponentRegistration> RegistrationsFor(
//                Service service,
//                Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
//            {
//                var components = _source.RegistrationsFor(service, registrationAccessor);
//                foreach (var c in components)
//                {
//                    var defs = c.Target.Services
//                        .OfType<TypedService>()
//                        .Select(x => x.ServiceType.GetGenericTypeDefinition());

//                    if (defs.Any(_types.Contains))
//                        yield return c;
//                }
//            }

//            public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
//        }
//    }
//    public class ProcessingModule  
//    {
//        public static void Load(ContainerBuilder builder,System.Reflection.Assembly notiEvent=null, System.Reflection.Assembly commandEvent=null)
//        {
//            builder.RegisterType<NexusDataContext>().InstancePerDependency();
//            builder.RegisterType<CommandHandlerExecutor>().AsImplementedInterfaces();
//            builder.RegisterType<CustomDependencyResolver>().AsImplementedInterfaces();
//            builder.RegisterType<EventStore>().AsImplementedInterfaces();
//            if(commandEvent != null)
//            builder.RegisterAssemblyTypes(commandEvent)
//                .Where(t => t.Name.EndsWith("EventHandler"))
//                .AsImplementedInterfaces().InstancePerDependency();
//            builder.RegisterType<EventBusKafka>().As<IEventBus>().InstancePerDependency();
//            builder.RegisterType<InMemoryEventBusSubscriptionsManager>().As<IEventBusSubscriptionsManager>().InstancePerDependency();
//            builder.RegisterType<DomainEventsDispatcher>()
//                .As<IDomainEventsDispatcher>()
//                .InstancePerLifetimeScope();
//            if(notiEvent!=null)
//            builder.RegisterAssemblyTypes(notiEvent)
//                .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();

//            //builder.RegisterGenericDecorator(
//            //    typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
//            //    typeof(INotificationHandler<>));

//            //builder.RegisterGenericDecorator(
//            //    typeof(DomainEventsDispatcherCommandHandlerDecorator<>),
//            //    typeof(IRequestHandler<,>));
//        }
//    }
//}
