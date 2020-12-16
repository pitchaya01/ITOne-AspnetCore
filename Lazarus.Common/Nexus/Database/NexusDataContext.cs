using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazarus.Common.Model;
using Microsoft.EntityFrameworkCore;

namespace Lazarus.Common.Nexus.Database
{
    public class NexusDataContext:DbContext
    {
        public NexusDataContext(DbContextOptions<NexusDataContext> options)
   : base(options)
        { }
        public DbSet<LogEventStore> LogEventStores { get; set; }
        public DbSet<KafkaConsumerConfig> KafkaConsumerConfig { get; set; }
        public DbSet<LogMessage> LogMessages { get; set; }
        public DbSet<FileUpload> FileUpload { get; set; }


    }
}
