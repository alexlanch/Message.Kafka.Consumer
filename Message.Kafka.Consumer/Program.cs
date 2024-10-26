using Confluent.Kafka;
using Message.Kafka.Consumer;
using Message.Kafka.Consumer.Domain.Interfaces;
using Message.Kafka.Consumer.Domain.Services;
using Message.Kafka.Consumer.Infrastructure.Repository;
using Message.Kafka.Consumer.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            Console.WriteLine($"Environment: {environment}");

            if (string.IsNullOrEmpty(environment))
            {
                Console.WriteLine("INFO: Environment variable not setted (ASPNETCORE_ENVIRONMENT)");
                Console.WriteLine("Default Environment: Development");
                environment = "Development";
            }

            config.AddJsonFile("appsettings.json", optional: true);
            config.AddJsonFile($"appsettings.{environment}.json", optional: true);

            config.AddEnvironmentVariables();

            config.AddCommandLine(args);
        })
    .ConfigureServices((hostContext, services) =>
    {

        //Settings
        services.Configure<KafkaConsumerSettings>(hostContext.Configuration.GetSection("KafkaConsumerSettings"));

        //Services
        services.AddTransient<IKafkaConsumerService, KafkaConsumerService>();
        
        

        //Repositories
        //services.AddTransient<IPersistenceRepository, PersistenceRepository>();
        services.AddTransient<IKafkaConsumerRepository, KafkaConsumerRepository>();


        services.AddHostedService<Worker>();
    })
    .Build();

//builder.Services.AddTransient<ILogsEntry, objLogsEntry>();

/* Configurar DbContext
builder.Services.AddDbContext<PersistenceRepository>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringAccessControl")), ServiceLifetime.Singleton); */

//builder.Services.AddHostedService<Worker>();

host.Run();
