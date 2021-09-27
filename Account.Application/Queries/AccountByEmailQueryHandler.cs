using Account.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Application
{
    public class AccountByEmailQueryHandler : IRequestHandler<AccountByEmailQuery, AccountResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountByEmailQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountResponse> Handle(AccountByEmailQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByEmail(request.AccountEmail);
            return new AccountResponse(account.Id, account.Owner.CustomerName, account.Balance.Value);
        }
    }
}