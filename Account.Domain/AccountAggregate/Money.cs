using Core;
using System.Collections.Generic;

namespace Account.Domain
{
    public class Money : ValueObject
    {
        public Money(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }

        public Money Subtract(decimal amount) => new Money(this.Value - amount);

        public Money Add(decimal amount) => new Money(this.Value + amount);

        public static Money Zero() => new Money(0);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
