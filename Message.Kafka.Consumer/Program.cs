using Message.Kafka.Consumer;
using Message.Kafka.Consumer.Domain.Interfaces;
using Message.Kafka.Consumer.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<ILogsEntry, objLogsEntry>();
//builder.Services.AddTransient<IPersistenceRepository, PersistenceRepository>();

// Configurar DbContext
builder.Services.AddDbContext<PersistenceRepository>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringAccessControl")), ServiceLifetime.Singleton);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
