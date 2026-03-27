using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject
{
    public class PharmacyResponseItem : ValueObject_
    {
        public string DrugName { get; private set; }
        public bool Available { get; private set; }
        public decimal? Price { get; private set; }

        private PharmacyResponseItem() // For Entity Framework Core
        {
            
        }

        public PharmacyResponseItem(string Dn, bool A, decimal? price)
        {
            DrugName = Dn;
            Available = A;
            Price = price;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DrugName;
            yield return Available;
            yield return Price;
        }
    }
}
