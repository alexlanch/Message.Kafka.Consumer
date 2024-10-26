using Confluent.Kafka;
using Message.Kafka.Consumer.Domain.Interfaces;
using Message.Kafka.Consumer.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Message.Kafka.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly IKafkaConsumerService _consumerService;

        public Worker(IKafkaConsumerService kafkaConsumerService)
        {
            _consumerService = kafkaConsumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                List<Domain.Models.Message> messageList = await _consumerService.GetMessage();
                

                foreach (var message in messageList)
                {
                    Console.WriteLine(message.Name);
                    Console.WriteLine(message.Lastname);
                    Console.WriteLine(message.Telephone);
                    //nombreservice.metodoGuardar(message)
                }
            }


        }
    }

}
