using Account.Domain;
using Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Application
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, bool>
    {
        private readonly IUnitOfWork<Domain.Account> _unitOfWork;
        private readonly ILogger _logger;

        public CreateAccountHandler(IUnitOfWork<Domain.Account> unitOfWork, ILogger<CreateAccountHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateAccountCommand request,
            CancellationToken cancellationToken)
        {
            var customer = new Customer(request.CustomerId, request.CustomerName);
            var account = new Domain.Account(customer, new Money(request.InitialCredit));

            await _unitOfWork.SaveEntitiesAsync(account);
            return true;
        }
    }
}