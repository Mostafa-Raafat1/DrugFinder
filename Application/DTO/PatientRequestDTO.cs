using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PatientRequestDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }

        public List<DrugItemDTO> Drugs { get; set; }

        public PatientRequestDto( int id, DateTime ca, string st, List<DrugItemDTO> di)
        {
            Id = id;
            CreatedAt = ca;
            Status = st;
            Drugs = di;
        }
    }
}
