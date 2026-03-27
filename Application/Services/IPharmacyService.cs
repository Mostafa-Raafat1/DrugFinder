using Application.Common;
using Application.DTO;

namespace Application.Services
{
    public interface IPharmacyService
    {
        Task<Result> RegisterPharmacyAsync(RegisterPharmacyDTO pharmacyDTO);
        Task<Result<List<GetNotificationDTO>>> GetNotificationsForPharmacyAsync();
        Task<Result<List<PharmacyRequestDTO>>> GetNearbyRequestsAsync();
    }
}
