using System;

namespace Account.Domain.Exceptions
{
    public class AccountDomainException : Exception
    {
        public AccountDomainException()
        { }

        public AccountDomainException(string message)
            : base(message)
        { }

        public AccountDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }


    public class AccountNotFoundException : AccountDomainException
    {
        public AccountNotFoundException(string Id) : base(String.Format("Account with Id/Email {0} is not found", Id))
        {
        }
    }

    public class AccountWithdrawalException : AccountDomainException
    {
        public AccountWithdrawalException(string id, decimal value) : base(String.Format("unable to withdrawn {0} Account with Id {1} ", value, id))
        {
        }
    }
}

