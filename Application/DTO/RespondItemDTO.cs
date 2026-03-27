using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class RespondItemDTO
    {
        public string DrugName { get; set; } 
        public bool Available { get; set; }                  
        public decimal? Price { get; set; }
    }
}
