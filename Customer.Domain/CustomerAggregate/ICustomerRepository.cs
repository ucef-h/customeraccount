using System.Threading.Tasks;

namespace Customer.Domain
{
    public interface ICustomerRepository
    {
        Task EnsureNotExistingCustomer(string name, string email);
        Task InsertAsync(Customer entity);

        Task UpdateAsync(Customer entity);
        Task<Customer> SelectAsync(string entityId);
    }
}
