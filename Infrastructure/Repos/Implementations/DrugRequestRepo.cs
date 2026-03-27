using Domain.Entity;
using Domain.Value_Object;
using Infrastructure.Persistence.DbContext;
using Infrastructure.Repos.Common;
using Infrastructure.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos.Implementations
{
    public class DrugRequestRepo : Repo<DrugRequest>, IDrugRequest
    {
        private readonly AppDbContext dbContext;

        public DrugRequestRepo(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DrugRequest> getDrugRequestByDomainId(Guid domainId)
        {
            return await dbContext.DrugRequests.FirstOrDefaultAsync(dr => dr.Id == domainId);
        }

        public async Task<List<DrugRequest>> getDrugRequestsByPatientIdAsync(int patientId)
        {
            return await dbContext.DrugRequests.Where(dr => dr.PatientId == patientId).ToListAsync();
        }

        public async Task<List<DrugRequest>> GetNearbyDrugRequestsAsync(Location location, double max)
        {
                    return await dbContext.DrugRequests
            .Where(p => p.Location.Point.Distance(location.Point) <= max)
            .OrderBy(p => p.Location.Point.Distance(location.Point))
            .ToListAsync();
        }
    }
}
