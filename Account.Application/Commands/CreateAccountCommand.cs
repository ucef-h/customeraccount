using MediatR;

namespace Account.Application
{
    public class CreateAccountCommand : IRequest<bool>
    {
        public string CustomerId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public decimal InitialCredit { get; }

        public CreateAccountCommand(string customerId, string customerName, string customerEmail, decimal initialCredit)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            InitialCredit = initialCredit;
        }
    }
}