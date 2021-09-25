using Account.Domain.Events;
using Account.Domain.Exceptions;
using Core;
using System;

namespace Account.Domain
{
    public class Account : BaseEntity, IAggregateRoot
    {
        private Account() { }

        public Account(Customer owner, Money initialCredit)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            this.Owner = owner;
            this.Balance = Money.Zero();

            this.AddAccountCreatedEvent(owner);
            this.AddDepositEvent(initialCredit);
        }

        public Customer Owner { get; private set; }

        public Money Balance { get; private set; }

        public void Withdraw(Money amount)
        {
            if (amount.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "amount cannot be negative");

            if (amount.Value > this.Balance.Value)
                throw new AccountWithdrawalException(this.Id, amount.Value);

            this.AddWithdrawalEvent(amount);
        }

        public void Deposit(Money amount)
        {
            if (amount.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "amount cannot be negative");

            this.AddDepositEvent(amount);
        }

        public void ReApplyAll()
        {
            foreach (var domainEvent in EventsHistory)
            {
                Apply(domainEvent);
            }
        }

        private void Apply(IDomainEvent @event)
        {
            switch (@event)
            {
                case AccountCreatedEvent c:
                    this.Balance = Money.Zero();
                    this.Owner = c.Owner;
                    break;
                case WithdrawalEvent w:
                    this.Balance = this.Balance.Subtract(w.Amount.Value);
                    break;
                case DepositEvent d:
                    this.Balance = this.Balance.Add(d.Amount.Value);
                    break;
            }
        }

        private void AddAccountCreatedEvent(Customer owner)
        {
            var accountCreatedEvent = new AccountCreatedEvent(this, owner);
            this.AddDomainEvent(accountCreatedEvent);
            this.Apply(accountCreatedEvent);
        }

        private void AddWithdrawalEvent(Money amount)
        {
            var withdrawalEvent = new WithdrawalEvent(this, amount);
            this.AddDomainEvent(withdrawalEvent);
            this.Apply(withdrawalEvent);
        }

        private void AddDepositEvent(Money amount)
        {
            if (amount == null) throw new ArgumentNullException();
            var depositEvent = new DepositEvent(this, amount);
            this.AddDomainEvent(depositEvent);
            this.Apply(depositEvent);
        }
    }
}