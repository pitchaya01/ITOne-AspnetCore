using ITOne_AspnetCore.Domain;
using Lazarus.Common.DAL;

namespace ITOne_AspnetCore.Repository
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        string GetCustomerName();
    }
}
