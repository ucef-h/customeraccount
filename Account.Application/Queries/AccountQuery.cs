using MediatR;

namespace Account.Application
{
    public class AccountQuery : IRequest<AccountResponse>
    {
        public AccountQuery(string accountId)
        {
            AccountId = accountId;
        }

        public string AccountId { get; }
    }
}