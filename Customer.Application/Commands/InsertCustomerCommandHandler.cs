using Core;
using Customer.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.Application
{
    public class InsertCustomerCommandHandler : IRequestHandler<InsertCustomerCommand, bool>
    {
        private readonly IUnitOfWork<Domain.Customer> _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<InsertCustomerCommandHandler> _logger;

        public InsertCustomerCommandHandler(
            IUnitOfWork<Domain.Customer> unitOfWork,
            ICustomerRepository customerRepository,
            ILogger<InsertCustomerCommandHandler> logger
        )
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(InsertCustomerCommand request,
            CancellationToken cancellationToken)
        {
            await _customerRepository.EnsureNotExistingCustomer(request.Name, request.Email);

            var customer = new Domain.Customer(request.Email, request.Name, request.Credits);
            await _unitOfWork.SaveEntitiesAsync(customer);
            return true;
        }
    }
}