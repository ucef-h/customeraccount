namespace Account.Application
{
    public class AccountResponse
    {
        public string Id { get; private set; }
        public string CustomerName { get; private set; }
        public decimal Balance { get; private set; }

        public AccountResponse(string id, string customerName, decimal balanceValue)
        {
            Id = id;
            CustomerName = customerName;
            Balance = balanceValue;
        }
    }
}