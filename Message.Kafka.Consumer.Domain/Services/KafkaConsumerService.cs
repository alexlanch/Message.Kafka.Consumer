using Message.Kafka.Consumer.Domain.Interfaces;
using Message.Kafka.Consumer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Message.Kafka.Consumer.Domain.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IKafkaConsumerRepository _kafkaConsumerRepository;


        public KafkaConsumerService(IKafkaConsumerRepository kafkaConsumerRepository) 
        {
            _kafkaConsumerRepository = kafkaConsumerRepository;


        }

        public async Task<List<Models.Message>> GetMessage()
        {
            List<Models.Message> messageList = await _kafkaConsumerRepository.GetMessage();



            return messageList;
        }

    }
}
