using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message.Kafka.Consumer.Domain.Interfaces;
using static Confluent.Kafka.ConfigPropertyNames;
using System.Runtime;
using Message.Kafka.Consumer.Infrastructure.Settings;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Message.Kafka.Consumer.Infrastructure.Repository
{
    public class KafkaConsumerRepository : IKafkaConsumerRepository
    {
        private readonly KafkaConsumerSettings _settings;
        private IConsumer<Ignore, string> consumer;

        public KafkaConsumerRepository(IOptions<KafkaConsumerSettings> settings)
        {
            _settings = settings.Value;
            CreateConsumer();
        }


        private void CreateConsumer()
        {
            consumer = new ConsumerBuilder<Ignore, string>(ConstructConfig(_settings.AutoCommit))
                 .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                 .SetStatisticsHandler((_, json) => { })
                 .SetPartitionsAssignedHandler((c, partitions) =>
                 {
                     Console.WriteLine($"Assigned partitions: [{string.Join(", ", partitions)}]");
                 })
                 .SetPartitionsRevokedHandler((c, partitions) =>
                 {
                     Console.WriteLine($"Revoking assignment: [{string.Join(", ", partitions)}]");
                 })
            .Build();

            consumer.Subscribe(_settings.Topic);
        }

        private ConsumerConfig ConstructConfig(bool enableAutoCommit) =>
                new ConsumerConfig
                {
                    BootstrapServers = "192.9.201.145:9092,192.9.201.146:9092,192.9.201.147:9092",
                    GroupId = _settings.ConsumerGroup,
                    StatisticsIntervalMs = 5000,
                    AutoCommitIntervalMs = _settings.CommitIntervalMs,
                    SessionTimeoutMs = 15000,
                    AutoOffsetReset = AutoOffsetReset.Latest,
                    EnableAutoCommit = enableAutoCommit,
                    EnablePartitionEof = true,
                    MaxPollIntervalMs = 600000

                };


        public Task<List<Domain.Models.Message>> GetMessage()
        {
            if (consumer == null)
                CreateConsumer();

            try
            {
                var executionDate = DateTime.UtcNow;
                List<Domain.Models.Message> topicEvents = new List<Domain.Models.Message>();

                while (CanRead(executionDate, topicEvents.Count))
                {
                    string topicMsg = string.Empty;

                    try
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));
                        if (consumeResult != null && consumeResult.Message != null)
                        {
                            topicMsg = consumeResult.Message.Value;

                            if (!string.IsNullOrEmpty(topicMsg))
                            {
                                Domain.Models.Message topicEvent = JsonConvert.DeserializeObject<Domain.Models.Message>(topicMsg);
                                topicEvents.Add(topicEvent);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} topicValue: {topicMsg}");
                    }

                }

                return Task.FromResult(topicEvents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.UtcNow.AddHours(-5).ToString("yyyy-MM-dd HH:mm:ss") + ex.Message);
                throw;
            }
        }

        public bool CanRead(DateTime executionDate, int eventsRead)
        {
            if (executionDate > DateTime.UtcNow.AddSeconds(-_settings.RecopilationTime) && eventsRead < _settings.MaxMessagesCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
