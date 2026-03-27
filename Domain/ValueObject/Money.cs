using Domain.Enum;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Money : ValueObject_
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { } // For EF Core
        public Money(decimal amount, Currency currency)
        {
            // You can add validation here if needed (e.g., non-negative amount)
            amount = amount;
            Currency = currency.ToString();
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
