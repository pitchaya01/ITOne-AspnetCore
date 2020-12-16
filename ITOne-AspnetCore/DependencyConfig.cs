using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using FluentValidation;
using ITOne_AspnetCore.Api.User.Database;
using ITOne_AspnetCore.Application.Command;
using ITOne_AspnetCore.Domain.Event;
using ITOne_AspnetCore.Infrastructure;
using ITOne_AspnetCore.Repository;
using Lazarus.Common.Application;
using Lazarus.Common.CQRS.Implement;
using Lazarus.Common.DI;
using Lazarus.Common.EventMessaging;
using Lazarus.Common.EventMessaging.EventStore;
using Lazarus.Common.EventMessaging.Implement;
using Lazarus.Common.infrastructure;
using Lazarus.Common.Infrastructure.Processing;
using Lazarus.Common.Nexus.Database;
using Lazarus.Common.Utilities;
using Lazarus.Common.Validator;
using MediatR;
using MediatR.Pipeline;
using RdKafka;
using Shared.Event;
using Shared.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ITOne_AspnetCore
{
    #region Implement
    public class RegisterEventModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {

            
            builder.RegisterType<CommandHandlerExecutor>().AsImplementedInterfaces();
                builder.RegisterType<CustomDependencyResolver>().AsImplementedInterfaces();
                builder.RegisterType<EventStore>().AsImplementedInterfaces();
                builder.RegisterAssemblyTypes(typeof(CustomerCreatedEventHandler).Assembly)
                    .Where(t => t.Name.EndsWith("EventHandler"))
                    .AsImplementedInterfaces().InstancePerDependency();
                builder.RegisterType<EventBusKafka>().As<IEventBus>().InstancePerDependency();
                builder.RegisterType<InMemoryEventBusSubscriptionsManager>().As<IEventBusSubscriptionsManager>().InstancePerDependency();
            }
        }
        public class RegisterServiceModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {

                builder.RegisterType<DbDataContext>().InstancePerLifetimeScope();
                builder.RegisterType<DbDataReadContext>().InstancePerLifetimeScope();
 
                builder.RegisterGeneric(typeof(RepositoryBase<>))
                    .AsImplementedInterfaces();
                builder.Register<ServiceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.Resolve(t);
                });
                builder.RegisterAssemblyTypes(typeof(CustomerRepository).Assembly)
                     .Where(t => t.Name.EndsWith("Repository"))
                     .AsImplementedInterfaces().InstancePerDependency();

                builder.RegisterAssemblyTypes(typeof(LogRepository).Assembly)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces().InstancePerDependency();
     
            }
        }

        public class SharedModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<NexusDataContext>().InstancePerDependency();
                builder.RegisterAssemblyTypes(typeof(Lazarus.Common.Authentication.UserService).Assembly)
                  .Where(t => t.Name.EndsWith("Service"))
                  .AsImplementedInterfaces().InstancePerDependency();

                builder.RegisterAssemblyTypes(typeof(MasterProxy).Assembly)
                  .Where(t => t.Name.EndsWith("Proxy"))
                  .AsImplementedInterfaces().InstancePerDependency();

                builder.RegisterAssemblyTypes(typeof(HelperService).Assembly)
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces().InstancePerDependency();

            }
        }
        public class ProcessingModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<DomainEventsDispatcher>()
                    .As<IDomainEventsDispatcher>()
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(typeof(CustomerCreatedEventHandler).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();


                builder.RegisterGenericDecorator(
                    typeof(DomainEventsDispatcherCommandHandlerDecorator<,>),
                    typeof(IRequestHandler<,>));

                builder.RegisterType<CommandsDispatcher>()
                    .As<ICommandsDispatcher>()
                    .InstancePerLifetimeScope();


            }
        }
        public class MediatorModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterSource(new ScopedContravariantRegistrationSource(
                    typeof(IRequestHandler<,>),
                    typeof(INotificationHandler<>),
                    typeof(IValidator<>)
                ));

                builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

                var mediatrOpenTypes = new[]
                {
            typeof(IRequestHandler<,>),
            //typeof(INotificationHandler<>),
            typeof(IValidator<>),
        };

                foreach (var mediatrOpenType in mediatrOpenTypes)
                {
                    builder
                        .RegisterAssemblyTypes(typeof(SaveCustomerCommandHandler).GetTypeInfo().Assembly)
                        .AsClosedTypesOf(mediatrOpenType)
                        .AsImplementedInterfaces();
                }

                builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
                builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

                builder.Register<ServiceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.Resolve(t);
                });

                builder.RegisterGeneric(typeof(CommandValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            }


        }
        public class ScopedContravariantRegistrationSource : IRegistrationSource
        {
            private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
            private readonly List<Type> _types = new List<Type>();

            public ScopedContravariantRegistrationSource(params Type[] types)
            {
                if (types == null)
                    throw new ArgumentNullException(nameof(types));
                if (!types.All(x => x.IsGenericTypeDefinition))
                    throw new ArgumentException("Supplied types should be generic type definitions");
                _types.AddRange(types);
            }


            public IEnumerable<IComponentRegistration> RegistrationsFor(Autofac.Core.Service service, Func<Autofac.Core.Service, IEnumerable<IComponentRegistration>> registrationAccessor)
            {
                var components = _source.RegistrationsFor(service, registrationAccessor);
                foreach (var c in components)
                {
                    var defs = c.Target.Services
                        .OfType<TypedService>()
                        .Select(x => x.ServiceType.GetGenericTypeDefinition());

                    if (defs.Any(_types.Contains))
                        yield return c;
                }
            }

            public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
        }

        public class DependencyConfig
        {
            public static void RegisterEvent()
            {
                #region ----------REGISTER EVENT------
                if (DomainEvents._Consumer != null)
                    DomainEvents._Consumer.Dispose();
                var config = new Config() { GroupId = AppConfigUtilities.GetAppConfig<string>("KAFKA_CONSUMER_GROUP_ID") };
                config.EnableAutoCommit = false;
                config.StatisticsInterval = TimeSpan.FromSeconds(10);
                DomainEvents._Consumer = new EventConsumer(config, AppConfigUtilities.GetAppConfig<string>("KAFKA_URL"));

                var eventBus = DomainEvents._Container.Resolve<IEventBus>();
                eventBus.Subscribe<CustomerCreatedEvent, IIntegrationEventHandler<CustomerCreatedEvent>>();
                eventBus.StartBasicConsume();
                #endregion
            }
        }
    #endregion
}
