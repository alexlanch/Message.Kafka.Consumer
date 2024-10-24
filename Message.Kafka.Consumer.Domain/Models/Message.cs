using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Kafka.Consumer.Domain.Models
{
    public class Message
    {
        int Id { get; set; }
        DateTime Timestamp { get; set; }
        string Messages { get; set; }
    }
}
