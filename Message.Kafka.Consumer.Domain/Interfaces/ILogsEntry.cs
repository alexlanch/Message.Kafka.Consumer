using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Kafka.Consumer.Domain.Interfaces
{
    public interface ILogsEntry
    {
        int Id { get; set; }
        DateTime Timestamp { get; set; }
        string Message { get; set; }
    }
}
