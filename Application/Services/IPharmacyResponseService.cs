using Application.Common;
using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IPharmacyResponseService
    {
        Task<Result<PharmacyResponseDTO>> CreatePharmacyResponseAsync(PharmacyResponseDTO DTO);
    }
}
