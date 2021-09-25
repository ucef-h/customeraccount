using Account.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Application
{
    public class AccountQueryHandler : IRequestHandler<AccountQuery, AccountResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse> Handle(AccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.SelectAsync(request.AccountId);
            return new AccountResponse(account.Id, account.owner.CustomerName, account.Balance.Value);
        }
    }
}