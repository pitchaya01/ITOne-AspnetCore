using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazarus.Common.EventMessaging
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private Consumer<string, string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumer = new Consumer<string, string>(this._consumerConfig);
            _consumer.OnPartitionsAssigned += (obj, partitions) =>
            {
                var fromBeginning = partitions.Select(p => new TopicPartitionOffset(p.Topic, p.Partition, Offset.Stored)).ToList();
                _consumer.Assign(fromBeginning);
            };
            this._consumer.Subscribe(topicName);
        }
        public string readMessage()
        {
            try
            {
                var consumeResult = this._consumer.Consume();
                return consumeResult.Value;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
