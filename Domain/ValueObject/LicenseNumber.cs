using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class LicenseNumber : ValueObject_
    {
        public string Number { get; private set; }

        private LicenseNumber() { } // For EF Core
        public LicenseNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("License number cannot be empty.");
            Number = number;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }
    }
}
