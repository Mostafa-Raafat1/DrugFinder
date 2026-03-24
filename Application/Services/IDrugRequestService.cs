using Application.Common;
using Application.DTO;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDrugRequestService
    {
        Task<Result> CreateDrugRequestAsync(List<CreateDrugRequestDTO> DTO);
    }
}
