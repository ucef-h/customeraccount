using Core;

namespace Account.Domain.Events
{
    public class AccountCreatedEvent : DomainEvent
    {
        private readonly Account _account;

        public AccountCreatedEvent() { }

        public AccountCreatedEvent(Account account, Customer owner)
        {
            _account = account;
            Owner = owner;
        }

        public Customer Owner { get; private set; }

        public Account Account()
        {
            return _account;
        }
    }
}
