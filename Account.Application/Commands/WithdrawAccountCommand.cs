using MediatR;

namespace Account.Application
{
    public class WithdrawAccountCommand : IRequest<bool>
    {
        public WithdrawAccountCommand(string accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }

        public string AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}