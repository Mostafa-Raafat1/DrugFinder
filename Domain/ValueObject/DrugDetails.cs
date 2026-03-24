using Domain.Enum;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class DrugDetails : ValueObject
    {
        public string DrugName { get; private set; }
        public string Strength { get; private set; }
        public Form Form { get; private set; }
        public int Quantity { get; private set; }


        private DrugDetails() { } // For EF Core
        public DrugDetails(string dN, string strength, Form form, int quantity)
        {
            DrugName = dN;
            Strength = strength;
            Form = form;
            Quantity = quantity;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DrugName;
            yield return Strength;
            yield return Form;
            yield return Quantity;

        }
    }
}
