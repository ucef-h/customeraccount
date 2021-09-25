using MediatR;

namespace Account.Application
{
    public class DepositAccountCommand : IRequest<bool>
    {
        public DepositAccountCommand(string accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }

        public string AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}