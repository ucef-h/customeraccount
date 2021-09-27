using Core;
using System.Collections.Generic;

namespace Account.Domain
{
    public class Customer : ValueObject
    {
        public Customer(string customerId, string customerEmail, string customerName)
        {
            CustomerId = customerId;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
        }

        public string CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        public string CustomerEmail { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerId;

            yield return CustomerName;

            yield return CustomerEmail;
        }
    }
}
