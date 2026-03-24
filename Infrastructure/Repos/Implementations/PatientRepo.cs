using Domain.Domain;
using Infrastructure.Persistence.DbContext;
using Infrastructure.Repos.Common;
using Infrastructure.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos.Implementations
{
    internal class PatientRepo : Repo<Patient>, IPatient
    {
        private readonly AppDbContext dbContext;

        public PatientRepo(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Patient getPatientByUserAppId(string appUserId)
        {
            return dbContext.Patients.FirstOrDefault(p => p.AppUserId == appUserId);
        }
    }
}
