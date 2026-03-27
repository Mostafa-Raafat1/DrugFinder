using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PharmacyRequestDTO
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public double DistanceKm { get; set; }

        public List<DrugItemDTO> Drugs { get; set; } = new();
    }
}
