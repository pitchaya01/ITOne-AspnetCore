using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;

namespace ITOne_AspnetCore.Api.User.Database
{
    public class DesignTimeDbContextReadFactory : IDesignTimeDbContextFactory<DbDataReadContext>
    {
        public DbDataReadContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DbDataContext>();
            var connectionString = configuration.GetConnectionString("CustomerDatabaseRead");
            builder.UseSqlServer(connectionString);
            return new DbDataReadContext(builder.Options);
        }
    }
}
