using Core;

namespace Common.IntegrationEvents
{
    public class CustomerCreatedIntegrationEvent : IntegrationEvent
    {
        public CustomerCreatedIntegrationEvent()
        {
        }

        public CustomerCreatedIntegrationEvent(string customerId, string customerName, string customerEmail,
            decimal initialCredit)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            InitialCredit = initialCredit;
        }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal InitialCredit { get; set; }
    }
}