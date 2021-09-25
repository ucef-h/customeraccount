using Core;

namespace Account.Domain.Events
{
    public class WithdrawalEvent : DomainEvent
    {
        private readonly Account _account;

        public WithdrawalEvent() { }

        public WithdrawalEvent(Account account, Money amount)
        {
            _account = account;
            Amount = amount;
        }

        public Money Amount { get; private set; }

        public Account Account()
        {
            return _account;
        }
    }
}
