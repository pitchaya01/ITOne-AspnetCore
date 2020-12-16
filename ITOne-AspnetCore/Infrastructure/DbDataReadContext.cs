using Microsoft.EntityFrameworkCore;

namespace ITOne_AspnetCore.Api.User.Database
{
    public class DbDataReadContext : DbDataContext
    {
        public DbDataReadContext(DbContextOptions<DbDataContext> options)
: base(options)
        { }
    }
}
