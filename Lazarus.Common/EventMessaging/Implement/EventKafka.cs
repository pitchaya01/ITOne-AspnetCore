using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Lazarus.Common.DI;
using Lazarus.Common.EventMessaging.EventStore;
using Lazarus.Common.Utilities;
using RdKafka;
using System.Collections.Generic;
using System.Linq;
using Lazarus.Common.CQRS.Command;
using Lazarus.Common.CQRS.Implement;
using Lazarus.Common.Interface;
using System.Threading;

namespace Lazarus.Common.EventMessaging.Implement
{
    public class EventBusKafka : IEventBus, IDisposable
    {
        public IEventStore _EventStore;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly int _retryCount;
        ICommandHandlerExecutor CommandHandlerExecutor { get; }

        public EventBusKafka(
            ICommandHandlerExecutor commandHandlerExecutor,
            IEventBusSubscriptionsManager subsManager, IEventStore eventStore)
        {
            CommandHandlerExecutor = commandHandlerExecutor;
            _EventStore = eventStore;
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _retryCount = 5;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;



        }

        public async Task Send<TCommand>(TCommand command) where TCommand : IntegrationEvent
        {
            await ProccessBus(command);
        }
        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {

        }
        async Task ProccessBus(ICommand command)
        {
            var commandType = command.GetType();
            var executorType = CommandHandlerExecutor.GetType();

            await (Task)executorType.GetMethod(nameof(ICommandHandlerExecutor.Execute))
                     .MakeGenericMethod(commandType)
                     .Invoke(CommandHandlerExecutor, new[] { command });
        }

        public void Publish(IntegrationEvent @event)
        {
            var env = AppConfigUtilities.GetAppConfig<string>("KAFKA_ENV");
            _EventStore.Persist(@event);

            var _config = new Config();

            var eventName = @event.GetType().Name;
            using (Producer producer = new Producer(_config, AppConfigUtilities.GetAppConfig<string>("KAFKA_URL")))

            using (Topic topic = producer.Topic(env + eventName))
            {
                var id = Encoding.UTF8.GetBytes(@event.AggregateId);
                byte[] data = Encoding.UTF8.GetBytes(@event.ToJSON());
                DeliveryReport deliveryReport = topic.Produce(data, id).Result;

            }



        }
        public void Publish(object obj,string _topic)
        {
            var env = AppConfigUtilities.GetAppConfig<string>("KAFKA_ENV");
    
            var _config = new Config();

      
            using (Producer producer = new Producer(_config, AppConfigUtilities.GetAppConfig<string>("KAFKA_URL")))

            using (Topic topic = producer.Topic(_topic))
            {
                var id = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
                byte[] data = Encoding.UTF8.GetBytes(obj.ToJSON());
                DeliveryReport deliveryReport = topic.Produce(data, id).Result;

            }



        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            //_logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());


            _subsManager.AddDynamicSubscription<TH>(eventName);

        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _subsManager.AddSubscription<T, TH>();


        }



        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            //_logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {

            _subsManager.Clear();
        }

        public void StartBasicConsume()
        {
            var env = AppConfigUtilities.GetAppConfig<string>("KAFKA_ENV");
            var consumer = DomainEvents._Consumer;


            consumer.OnPartitionsAssigned += (obj, partitions) =>
            {

                var fromBeginning = partitions.Select(p => new RdKafka.TopicPartitionOffset(p.Topic, p.Partition, RdKafka.Offset.Stored)).ToList();
                consumer.Assign(fromBeginning);
            };
            consumer.OnMessage += Consumer_OnMessage;
            var events = this._subsManager.Events.Select(s => env + s).ToList();

            consumer.Subscribe(events);
            consumer.Start();
        }

        private void Consumer_OnMessage(object sender, Message e)
        {
            var isCommitSuccess = false;
            var i = 1;
            while (!isCommitSuccess)
            {
                if (i > 10)
                {
                    isCommitSuccess = true;
                    return;
                }
                var env = AppConfigUtilities.GetAppConfig<string>("KAFKA_ENV");
                var log = DomainEvents._Container.Resolve<ILogRepository>();
                string text = Encoding.UTF8.GetString(e.Payload, 0, e.Payload.Length);
                var eStore = DomainEvents._Container.Resolve<IEventStore>();
                var id = Encoding.UTF8.GetString(e.Key, 0, e.Key.Length);
                var gKafka = AppConfigUtilities.GetAppConfig<string>("KAFKA_CONSUMER_GROUP_ID");
                var envName = Environment.MachineName;
              
                try
                {

                    var topic = e.Topic.Replace(env, "");
                    ProcessEvent(topic, text).Wait();

                    DomainEvents._Consumer.Commit(e).Wait();


                    eStore.Commit(id, envName);
                    isCommitSuccess = true;
                }
                catch (Exception exception)
                {

                    eStore.CommitedFail(id, $"({i})" + exception.GetMessageError());
                    log.Error($"({i})[{e.Topic}]" + exception.GetMessageError(), exception.StackTrace, "Consumer_OnMessage", null);
                    isCommitSuccess = false;
                    i++;
                    Thread.Sleep(5000);
                }
            }


        }



        private async Task ProcessEvent(string eventName, string message)
        {
            //_logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = DomainEvents._Container.BeginLifetimeScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {

                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

                    }
                }
            }
            else
            {
                //_logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}