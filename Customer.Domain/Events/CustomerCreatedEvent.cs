using Core;

namespace Customer.Domain.Events
{
    public class CustomerCreatedEvent : DomainEvent
    {
        private readonly Customer _customer;

        public CustomerCreatedEvent()
        {
        }

        public CustomerCreatedEvent(Customer customer, string customerEmail, string customerName, decimal initialCredit)
        {
            _customer = customer;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            InitialCredit = initialCredit;
        }

        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
        public decimal InitialCredit { get; private set; }

        public Customer Customer()
        {
            return _customer;
        }
    }
}