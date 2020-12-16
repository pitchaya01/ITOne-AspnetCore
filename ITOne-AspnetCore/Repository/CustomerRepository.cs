using ITOne_AspnetCore.Api.User.Database;
using ITOne_AspnetCore.Domain;
using ITOne_AspnetCore.Infrastructure;
using Lazarus.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public DbDataContext _db;
        public DbDataReadContext _dbRead;
        public CustomerRepository(DbDataContext db,DbDataReadContext dbRead):base(db, dbRead)
        {
            _db = db;
            _dbRead = dbRead;
        }
        [ReadUnCommited]
        public string GetCustomerName()
        {

            var cus = _dbRead.Customers.FirstOrDefault();

            return cus?.Name;   
        }
    }
}
