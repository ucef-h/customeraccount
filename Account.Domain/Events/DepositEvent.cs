using Core;

namespace Account.Domain.Events
{
    public class DepositEvent : DomainEvent
    {
        private readonly Account _account;

        public DepositEvent() { }

        public DepositEvent(Account account, Money amount)
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
