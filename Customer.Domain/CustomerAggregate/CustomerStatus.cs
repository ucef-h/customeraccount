using Core;

namespace Customer.Domain
{
    public class CustomerStatus : Enumeration
    {
        public static CustomerStatus CustomerCreated = new CustomerStatus(1, nameof(CustomerCreated).ToLowerInvariant());

        protected CustomerStatus(int id, string name) : base(id, name)
        {
        }
    }
}
