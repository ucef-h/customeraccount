using MediatR;

namespace Customer.Application
{
    public class InsertCustomerCommand : IRequest<bool>
    {
        public InsertCustomerCommand(string name, string email, decimal credits)
        {
            Name = name;
            Email = email;
            Credits = credits;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Credits { get; set; }
    }
}