using Domain.Entity;
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
    public class DrugRequestRepo : Repo<DrugRequest>, IDrugRequest
    {
        private readonly AppDbContext dbContext;

        public DrugRequestRepo(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
