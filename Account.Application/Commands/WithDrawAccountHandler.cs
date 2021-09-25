using Account.Domain;
using Core;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Application
{
    public class WithdrawAccountCommandHandler : IRequestHandler<WithdrawAccountCommand, bool>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork<Domain.Account> _unitOfWork;

        public WithdrawAccountCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork<Domain.Account> unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(WithdrawAccountCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _accountRepository.SelectAsync(request.AccountId);
            account.Withdraw(new Money(request.Amount));
            await _unitOfWork.SaveEntitiesAsync(account);
            return true;
        }
    }
}