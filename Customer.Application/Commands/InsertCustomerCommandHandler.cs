using Core;
using Customer.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.Application
{
    public class InsertCustomerCommandHandler : IRequestHandler<InsertCustomerCommand, bool>
    {
        private readonly IUnitOfWork<Domain.Customer> _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public InsertCustomerCommandHandler(
            IUnitOfWork<Domain.Customer> unitOfWork,
            ICustomerRepository customerRepository
        )
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(InsertCustomerCommand request,
            CancellationToken cancellationToken)
        {
            await _customerRepository.EnsureNotExistingCustomer(request.Name, request.Email);

            var customer = new Domain.Customer(request.Email, request.Name, request.Credits);
            return await _unitOfWork.SaveEntitiesAsync(customer);
        }
    }
}