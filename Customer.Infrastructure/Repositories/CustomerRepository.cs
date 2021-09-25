using Core;
using Customer.Domain;
using Customer.Domain.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Customer.Infrastructure.Repositories
{
    public class CustomerRepository : EntityRepository<Domain.Customer, string>, ICustomerRepository
    {
        private readonly IMongoCollection<Domain.Customer> _customer;

        public CustomerRepository(IMongoClient client)
        {
            _customer = client.GetDatabase("customer-account").GetCollection<Domain.Customer>(EntityName);
        }

        public sealed override string EntityName => "customer";

        public async Task EnsureNotExistingCustomer(string name, string email)
        {
            var customer = await _customer
                .Find(e => e.CustomerName.Equals(name) && e.CustomerEmail.Equals(email))
                .FirstOrDefaultAsync();
            if (customer != null)
            {
                throw new CustomerExistsException(name, email);
            }
        }

        public async Task InsertAsync(Domain.Customer entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            PreInsertEntity(entity);
            await _customer.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Domain.Customer entity)
        {
            PreUpdateEntity(entity);
            await _customer.ReplaceOneAsync(filter => filter.Id.Equals(entity.Id), entity);
        }

        public async Task<Domain.Customer> SelectAsync(string entityId)
        {
            return await _customer.Find(e => e.Id.Equals(entityId)).FirstOrDefaultAsync();
        }
    }
}