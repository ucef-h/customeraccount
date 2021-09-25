using System;

namespace Customer.Application
{
    public class RequestTransient
    {
        private readonly string _transactionId;

        public RequestTransient()
        {
            _transactionId = Guid.NewGuid().ToString();
        }

        public string TransactionId
        {
            get { return _transactionId; }
        }

    }
}
