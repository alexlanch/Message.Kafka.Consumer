using Message.Kafka.Consumer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Message.Kafka.Consumer.Infrastructure.Repository
{
    public class PersistenceRepository : DbContext
    {
        // logica para guardar en la bae de datos, interfaces para inyectar en las otra capas.
        public PersistenceRepository(DbContextOptions<PersistenceRepository> options) : base(options) { }

        public DbSet<objLogsEntry> LogsEntry { get; set; }
    }
}
