using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Message.Kafka.Consumer.Domain;
using Message.Kafka.Consumer.Domain.Interfaces;

namespace Message.Kafka.Consumer.Infrastructure.Repository
{
    public class objLogsEntry : ILogsEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Message { get; set; }
    }
}
