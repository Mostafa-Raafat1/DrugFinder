using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CreateDrugRequestDTO
    {
        [Required(ErrorMessage ="Request must have a Drug Name")]
        public string DrugName { get; set; }
        public string Strength { get; set; }
        [Range(1,6,ErrorMessage ="Form is not correct")]
        public Form Form { get; set; }
        [Range(1,int.MaxValue, ErrorMessage ="Enter a correct quantity")]
        public int Quantity { get; set; }

    }
}
