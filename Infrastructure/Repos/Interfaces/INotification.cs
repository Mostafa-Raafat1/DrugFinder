using Domain.Entity;
using Infrastructure.Repos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos.Interfaces
{
    public interface INotification : IRepo<Notification>
    {
        Task<List<Notification>> GetNotificationsForPharmacy(int PhId);
    }
}
