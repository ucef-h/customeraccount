using MediatR;

namespace Account.Application
{
    public class AccountQuery : IRequest<AccountResponse>
    {
        public AccountQuery(string customerEmail)
        {
            CustomerEmail = customerEmail;
        }

        public string CustomerEmail { get; }
    }
}