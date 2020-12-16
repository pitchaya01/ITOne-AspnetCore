using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITOne_AspnetCore.Domain;

namespace ITOne_AspnetCore.Api.User.Database
{
    public class DbDataContext:DbContext
    {
        public DbDataContext(DbContextOptions<DbDataContext> options)
   : base(options)
        { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
 

    }
}
