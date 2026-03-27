using Domain.Entity;
using Domain.Value_Object;
using Infrastructure.Repos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos.Interfaces
{
    public interface IDrugRequest : IRepo<DrugRequest>
    {
        Task<List<DrugRequest>> GetNearbyDrugRequestsAsync(Location location, double max);
        Task<DrugRequest> getDrugRequestByDomainId(Guid domainId);
        Task<List<DrugRequest>> getDrugRequestsByPatientIdAsync(int patientId);
    }
}
