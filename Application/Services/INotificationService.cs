using Application.Common;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface INotificationService
    {
        Task<Result> SendNotificationToNearbyPharmaciesAsync(Location location, string Message);

    }
}
