using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Kafka.Consumer.Infrastructure.Settings
{
    public class KafkaConsumerSettings
    {
        public string[] Topic { get; set; }
        public string BrokersList { get; set; }
        public string ConsumerGroup { get; set; }
        public int? CommitIntervalMs { get; set; }
        public bool AutoCommit { get; set; }
        public int RecopilationTime { get; set; }
        public int MaxMessagesCount { get; set; }
    }
}
