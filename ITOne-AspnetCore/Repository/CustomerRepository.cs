using ITOne_AspnetCore.Domain;
using ITOne_AspnetCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITOne_AspnetCore.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public string GetCustomerName()
        {
            var cus = this._DbRead.Customers.FirstOrDefault();

            return "";   
        }
    }
}
