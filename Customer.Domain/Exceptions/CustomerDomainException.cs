using System;

namespace Customer.Domain.Exceptions
{
    public class CustomerDomainException : Exception
    {
        public CustomerDomainException()
        { }

        public CustomerDomainException(string message)
            : base(message)
        { }

        public CustomerDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }


    public class CustomerNotFoundException : CustomerDomainException
    {
        public CustomerNotFoundException(string Id) : base(String.Format("Customer with Id {0} is not found", Id))
        {
        }
    }
    public class CustomerExistsException : CustomerDomainException
    {
        public CustomerExistsException(string name, string email)
            : base($"Customer with name {name} and email {email} already exists")
        {
        }
    }
}
