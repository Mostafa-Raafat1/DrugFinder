using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PharmacyResponseDTO
    {
        public int RequestId { get; set; }               
        public DateTime RespondedAt { get; set; }
        public List<RespondItemDTO> Items { get; set; } = new();
    }
}
