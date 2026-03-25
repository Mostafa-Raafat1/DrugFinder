using Domain.Entity;
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
    }
}
