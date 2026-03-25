using Application.Common;
using Application.DTO;
using Domain.Entity;
using Domain.Factory;
using Domain.Value_Object;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class NotificationService :INotificationService
    {
        private readonly IUnitOfWork uow;

        public NotificationService(IUnitOfWork Uow)
        {
            uow = Uow;
        }

        public async Task<Result<List<GetNotificationDTO>>> getNotificationForPharmacyAsync(int PhId)
        {
            var notfications = await uow.Notification.GetNotificationsForPharmacy(PhId);
            if (notfications == null || notfications.Count == 0)
            {
                return Result<List<GetNotificationDTO>>.Failure("No notifications found yet");
            }

            var notificationDTOs = notfications.Select(n => new GetNotificationDTO
            {
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            }).ToList();

            return Result<List<GetNotificationDTO>>.Success(notificationDTOs);
        }

        public async Task<Result> SendNotificationToNearbyPharmaciesAsync(Location location, string Message)
        {
            var nearbyPharmacies = await uow.Pharmacy.GetNearByPharmacies(location, 3000);

            if(nearbyPharmacies == null)
            {
                return Result.Failure("No Nearby pharmacies");
            }

            foreach (var pharmacy in nearbyPharmacies)
            {
                var notification = NotificationFactory.ForPharmacy(pharmacy.DBId, Message); // Use the factory to create the valid notification type
                await uow.Notification.Create(notification);
            }
            if (await uow.SaveChangesAsync() > 0)
            {
                return Result.Success();
            }
            return Result.Failure("Failed to send notification to pharmacy.");
        }
    }
}
