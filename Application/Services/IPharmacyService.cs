using Application.Common;
using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IPharmacyService
    {
        Task<Result> RegisterPharmacyAsync(RegisterPharmacyDTO pharmacyDTO);
        Task<Result<List<GetNotificationDTO>>> GetNotificationsForPharmacyAsync();
    }
}
