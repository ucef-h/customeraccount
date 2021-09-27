using MediatR;

namespace Account.Application
{
    public class AccountByEmailQuery : IRequest<AccountResponse>
    {
        public AccountByEmailQuery(string email)
        {
            AccountEmail = email;
        }

        public string AccountEmail { get; }
    }
}