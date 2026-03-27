using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class DrugItemDTO
    {
        public string Name { get; set; }
        public string? Strength { get; set; }
        public string? Form { get; set; }
        public int Quantity { get; set; }
        public DrugItemDTO(string n, string s, string f, int q )
        {
            Name = n;
            Strength = s;
            Form = f;
            Quantity = q;
        }
    }
}
