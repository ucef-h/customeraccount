using Customer.Domain.Events;
using NUnit.Framework;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Customer.Domain.Test
{
    public class CustomerTests
    {
        const string Name = "CustomerTest";
        const string Email = "test@example.com";
        const decimal Credits = 20m;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CustomerData()
        {
            var customer = new Customer(Email, Name, Credits);

            Assert.AreSame(customer.DomainEvents.First().GetType(), new CustomerCreatedEvent().GetType());
            Assert.AreEqual(customer.DomainEvents.Count, 1);

            Assert.AreEqual(customer.CustomerName, Name);
            Assert.AreEqual(customer.CustomerEmail, Email);
            Assert.AreEqual(customer.InitialCredit, Credits);
        }

        [Test]
        public void CustomerDataFromDomainEvents()
        {
            var customer = new Customer(Email, Name, Credits);

            var newCustomer = RuntimeHelpers.GetUninitializedObject(typeof(Customer)) as Customer;
            //newCustomer.SetPrivateFieldValue("_domainEvents", new List<DomainEvent>(customer.DomainEvents));
            newCustomer.AddDomainEvent(customer.DomainEvents.First());
            newCustomer.ArchiveDomainEvents();
            Assert.NotNull(newCustomer);

            Assert.Null(newCustomer.CustomerName);
            Assert.Null(newCustomer.CustomerEmail);
            Assert.Zero(newCustomer.InitialCredit);

            newCustomer.ReApplyAll();

            Assert.AreEqual(newCustomer.CustomerName, customer.CustomerName);
            Assert.AreEqual(newCustomer.CustomerEmail, customer.CustomerEmail);
            Assert.AreEqual(newCustomer.InitialCredit, customer.InitialCredit);

            Assert.Pass();
        }
    }
}