using Core;
using Customer.Domain.Events;
using System;

namespace Customer.Domain
{
    public class Customer : BaseEntity, IAggregateRoot
    {
        public Customer(string customerEmail, string customerName, decimal initialCredit)
        {
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            InitialCredit = initialCredit;
            SetStatus(CustomerStatus.CustomerCreated);
            AddCustomerCreatedEvent(customerEmail, customerName, initialCredit);
        }

        public string CustomerEmail { get; private set; }

        public string CustomerName { get; private set; }

        public decimal InitialCredit { get; private set; }

        public CustomerStatus Status { get; private set; }

        public DateTime StatusDate { get; private set; }

        public void SetStatus(CustomerStatus status)
        {
            this.Status = status;
            this.StatusDate = DateTime.UtcNow;
        }

        private void AddCustomerCreatedEvent(string customerEmail, string customerName, decimal initialCredit)
        {
            var customerCreatedEvent = new CustomerCreatedEvent(this, customerEmail, customerName, initialCredit);
            this.AddDomainEvent(customerCreatedEvent);
        }

        public bool PositiveInitialCredit()
        {
            return InitialCredit > default(decimal);
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
                case CustomerCreatedEvent c:
                    this.CustomerEmail = c.CustomerEmail;
                    this.CustomerName = c.CustomerName;
                    this.InitialCredit = c.InitialCredit;
                    break;
            }
        }
    }
}