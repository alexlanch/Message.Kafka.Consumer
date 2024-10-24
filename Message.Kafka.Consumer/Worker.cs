using Confluent.Kafka;
using Message.Kafka.Consumer.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Message.Kafka.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly string _connectionString = string.Empty;
        private readonly ILogger<Worker> _logger;
        private readonly PersistenceRepository _context;
        private readonly IConsumer<Null, string> _consumer;

        public Worker(ILogger<Worker> logger, PersistenceRepository context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _connectionString = configuration.GetConnectionString("ConnectionStringAccessControl") ?? string.Empty;

            // Configuración del consumidor de Kafka
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "192.9.201.145:9092,192.9.201.146:9092,192.9.201.147:9092",
                GroupId = "traffic-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var connectionSql = new SqlConnection(_connectionString))
            {
                _consumer.Subscribe("tracking.traffic.networkpktlistenertest");  // Suscribirse al topic de Kafka

                // Abrir la conexión de SQL Server de forma asíncrona
                await connectionSql.OpenAsync(stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var logMessage = $"Worker running at: {DateTimeOffset.Now}";
                    _logger.LogInformation(logMessage);

                    try
                    {
                        // Consumir mensajes de Kafka
                        var consumeResult = _consumer.Consume(stoppingToken);
                        var kafkaMessage = consumeResult.Message.Value;

                        // Insertar el mensaje consumido en la base de datos
                        var objLogsEntry = new objLogsEntry
                        {
                            Timestamp = DateTime.Now,
                            Message = kafkaMessage  // Suponiendo que hay un campo Message en la tabla LogsEntry
                        };

                        _context.LogsEntry.Add(objLogsEntry);
                        await _context.SaveChangesAsync(stoppingToken);

                        _logger.LogInformation($"Mensaje consumido e insertado en la base de datos: {kafkaMessage}");
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error al consumir mensaje: {e.Error.Reason}");
                    }

                    // Espera antes de continuar con la siguiente iteración
                    await Task.Delay(30000, stoppingToken);
                }
            }

        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }

}
