using Domain.Entity;
using Domain.Value_Object;
using Infrastructure.Repos.Common;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos.Interfaces
{
    public interface IPharmacy : IRepo<Pharmacy>
    {
        public Task<List<Pharmacy>> GetNearByPharmacies(Location location, double radiueMeters);
        public Task<Pharmacy> GetPharmacyByUserIdAsync(string userId);
    }
}
