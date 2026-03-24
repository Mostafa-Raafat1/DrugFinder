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
    internal class PharmacyRepo : Repo<Pharmacy>, IPharmacy
    {
        private readonly AppDbContext dbContext;

        public PharmacyRepo(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Pharmacy>> GetNearByPharmacies(Location location, double radiueMeters)
        {
            return await dbContext.Pharmacies
                .Where(p => p.Location.Point.Distance(location.Point) <= radiueMeters)
                .OrderBy(p => p.Location.Point.Distance(location.Point))
                .ToListAsync();
        }
    }
}
