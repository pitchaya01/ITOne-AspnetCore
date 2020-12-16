using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;

namespace ITOne_AspnetCore.Api.User.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbDataContext>
    {
        public DbDataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DbDataContext>();
            var connectionString = configuration.GetConnectionString("CustomerDatabase");
            builder.UseSqlServer(connectionString);
            return new DbDataContext(builder.Options);
        }
    }
}
