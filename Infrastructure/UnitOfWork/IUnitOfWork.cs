using Infrastructure.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPatient Patient { get; }
        IPharmacy Pharmacy { get; }
        IDrugRequest DrugRequest { get; }
        INotification Notification { get; }
        IPharamcyResponse PharamcyResponse { get; }
        Task<int> SaveChangesAsync();
    }
}
