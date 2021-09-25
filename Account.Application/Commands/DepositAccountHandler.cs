using Account.Domain;
using Core;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Application
{
    public class DepositAccountCommandHandler : IRequestHandler<DepositAccountCommand, bool>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<Domain.Account> _unitOfWork;

        public DepositAccountCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork<Domain.Account> unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DepositAccountCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _accountRepository.SelectAsync(request.AccountId);
            account.Deposit(new Money(request.Amount));
            await _unitOfWork.SaveEntitiesAsync(account);
            return true;
        }
    }
}