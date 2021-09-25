using Core;
using Customer.Domain;
using MediatR;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Customer.Infrastructure
{
    public class CustomerDbContext : IUnitOfWork<Domain.Customer>
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRepository;

        public CustomerDbContext(
            ICustomerRepository customerRepository,
            IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
        }

        public async Task<bool> SaveEntitiesAsync(Domain.Customer entity)
        {
            bool isTransient = await CheckIsTransient(entity);

            await _mediator.DispatchDomainEventsAsync(entity);

            if (isTransient)
            {
                await _customerRepository.InsertAsync(entity);
            }
            else
            {
                await _customerRepository.UpdateAsync(entity);
            }

            return true;
        }

        private async Task<bool> CheckIsTransient(Domain.Customer entity)
        {
            bool isTransient = string.IsNullOrEmpty(entity.Id);
            if (isTransient) entity.Id = ObjectId.GenerateNewId().ToString();
            else isTransient = await _customerRepository.SelectAsync(entity.Id) == null;
            return isTransient;
        }
    }
}