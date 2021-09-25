using Account.Domain.Events;
using Account.Domain.Exceptions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Account.Domain.Test
{
    public class AccountTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AccountInit()
        {
            var account = new Account(new Customer("", "", ""), new Money(3));

            Assert.AreSame(account.DomainEvents.First().GetType(), new AccountCreatedEvent().GetType());
            Assert.AreSame(account.DomainEvents.ElementAt(1).GetType(), new DepositEvent().GetType());

            Assert.AreEqual(((DepositEvent)account.DomainEvents.ElementAt(1)).Amount, account.Balance);
            Assert.AreEqual(((DepositEvent)account.DomainEvents.ElementAt(1)).Amount, new Money(3));

            Assert.AreEqual(account.Balance, new Money(3));
            Assert.AreEqual(account.Balance.Value, 3);
        }

        [Test]
        public void AccountTestDeposit()
        {
            var account = new Account(new Customer("", "", ""), new Money(3));

            account.Deposit(new Money(2));
            account.Deposit(new Money(4));

            Assert.AreSame(account.DomainEvents.ElementAt(2).GetType(), new DepositEvent().GetType());
            Assert.AreSame(account.DomainEvents.ElementAt(3).GetType(), new DepositEvent().GetType());

            Assert.AreEqual(((DepositEvent)account.DomainEvents.ElementAt(2)).Amount, new Money(2));
            Assert.AreEqual(((DepositEvent)account.DomainEvents.ElementAt(3)).Amount, new Money(4));


            Assert.AreEqual(account.Balance, new Money(9));
            Assert.AreEqual(account.Balance.Value, 9);
        }

        [Test]
        public void AccountWithdrawal()
        {
            var account = new Account(new Customer("", "", ""), new Money(10));

            account.Withdraw(new Money(2));
            account.Withdraw(new Money(4));

            Assert.AreSame(account.DomainEvents.ElementAt(2).GetType(), new WithdrawalEvent().GetType());
            Assert.AreSame(account.DomainEvents.ElementAt(3).GetType(), new WithdrawalEvent().GetType());

            Assert.AreEqual(((WithdrawalEvent)account.DomainEvents.ElementAt(2)).Amount, new Money(2));
            Assert.AreEqual(((WithdrawalEvent)account.DomainEvents.ElementAt(3)).Amount, new Money(4));

            Assert.AreEqual(account.Balance, new Money(4));
            Assert.AreEqual(account.Balance.Value, 4);
        }

        [Test]
        public void AccountDepositAndWithdrawal()
        {
            var account = new Account(new Customer("", "", ""), new Money(10));

            account.Withdraw(new Money(2));
            account.Deposit(new Money(4));
            account.Withdraw(new Money(8));
            account.Deposit(new Money(3));

            Assert.AreEqual(account.DomainEvents.Count, 6);
            Assert.AreEqual(account.DomainEvents.OfType<DepositEvent>().Count(), 3);
            Assert.AreEqual(account.DomainEvents.OfType<WithdrawalEvent>().Count(), 2);

            Assert.AreEqual(account.Balance, new Money(7));
            Assert.AreEqual(account.Balance.Value, 7);
        }

        [Test]
        public void AccountReApply()
        {
            var account = new Account(new Customer("email", "id", "name"), new Money(10));

            account.Withdraw(new Money(2));
            account.Deposit(new Money(4));
            account.Withdraw(new Money(8));
            account.Deposit(new Money(3));

            var newAccount = RuntimeHelpers.GetUninitializedObject(typeof(Account)) as Account;

            Assert.NotNull(newAccount);

            foreach (var domainEvent in account.DomainEvents)
            {
                newAccount.AddDomainEvent(domainEvent);
            }
            newAccount.ArchiveDomainEvents();

            Assert.Null(newAccount.Owner);
            Assert.Null(newAccount.Balance);

            newAccount.ReApplyAll();

            Assert.AreEqual(newAccount.Owner.CustomerId, "id");
            Assert.AreEqual(newAccount.Owner.CustomerName, "name");

            Assert.AreEqual(account.DomainEvents.Count, 6);
            Assert.AreEqual(account.DomainEvents.OfType<DepositEvent>().Count(), 3);
            Assert.AreEqual(account.DomainEvents.OfType<WithdrawalEvent>().Count(), 2);

            Assert.AreEqual(account.Balance, new Money(7));
            Assert.AreEqual(account.Balance.Value, 7);
        }

        [Test]
        public void AccountOverWithdraw()
        {
            var account = new Account(new Customer("", "", ""), new Money(10));
            Assert.Throws<AccountWithdrawalException>(() => account.Withdraw(new Money(11)));
        }

        [Test]
        public void AccountWithdrawNegative()
        {
            var account = new Account(new Customer("", "", ""), new Money(10));
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Withdraw(new Money(-1)));
        }

        [Test]
        public void AccountDepositNegative()
        {
            var account = new Account(new Customer("", "", ""), new Money(10));
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Deposit(new Money(-1)));
        }

        [Test]
        public void AccountCustomerNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(null, new Money(10)));
        }

        [Test]
        public void AccountMoneyNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(new Customer("", "", ""), null));
        }
    }
}