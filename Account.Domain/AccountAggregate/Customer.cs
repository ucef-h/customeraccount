using Core;
using System.Collections.Generic;

namespace Account.Domain
{
    public class Customer : ValueObject
    {
        public Customer(string email, string customerId, string customerName)
        {
            CustomerEmail = email;
            CustomerId = customerId;
            CustomerName = customerName;
        }

        public string CustomerEmail { get; private set; }

        public string CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerId;

            yield return CustomerName;
        }
    }
}
