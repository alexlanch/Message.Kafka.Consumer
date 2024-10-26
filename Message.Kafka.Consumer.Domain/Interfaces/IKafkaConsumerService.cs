using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Kafka.Consumer.Domain.Interfaces
{
    public interface IKafkaConsumerService
    {
        Task<List<Models.Message>> GetMessage();
    }
}
