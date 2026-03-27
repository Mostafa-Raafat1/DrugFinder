using Application.Common;
using Application.DTO;
using Application.Identity;
using Application.Services;
using Application.UserContext;
using Domain.Entity;
using Domain.Value_Object;
using Infrastructure.UnitOfWork;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;
        private readonly INotificationService notificationService;
        private readonly IUserContext userContext;

        public PharmacyService(IUnitOfWork uow, IUserService userService,
            INotificationService notificationService, IUserContext userContext)
        {
            this.uow = uow;
            this.userService = userService;
            this.notificationService = notificationService;
            this.userContext = userContext;
        }

        public async Task<Result<List<PharmacyRequestDTO>>> GetNearbyRequestsAsync()
        {
            var UserContextId = userContext.UserId;
            var pharmacy = await uow.Pharmacy.GetPharmacyByUserIdAsync(UserContextId);
            if (pharmacy == null)
            {
                return Result<List<PharmacyRequestDTO>>.Failure("Pharmacy not found");
            }

            var nearbyRequests = await uow.DrugRequest.GetNearbyDrugRequestsAsync(pharmacy.Location, 3000);
            if(nearbyRequests == null || nearbyRequests.Count == 0)
            {
                return Result<List<PharmacyRequestDTO>>.Failure("No nearby drug requests found");
            }
            var requestDTOs = nearbyRequests.Select(r => new PharmacyRequestDTO
            {
                Id = r.DBId,
                PatientName = r.PatientName,
                CreatedAt = r.RequestTime,
                DistanceKm = Math.Round(pharmacy.Location.CalculateDistance(r.Location) / 1000.0, 1), // get distance convert it Km and round to 1 decimal place
                Drugs = r.DrugDetails.Select(d => new DrugItemDTO
                (
                    d.DrugName,
                    d.Strength,
                    d.Form.ToString(),
                    d.Quantity
                )).ToList()
            }).ToList();

            return Result<List<PharmacyRequestDTO>>.Success(requestDTOs);
        }

        public async Task<Result<List<GetNotificationDTO>>> GetNotificationsForPharmacyAsync()
        {
            var pharmacy = await uow.Pharmacy.GetPharmacyByUserIdAsync(userContext.UserId);
            if (pharmacy == null)
            {
                return Result<List<GetNotificationDTO>>.Failure("Pharmacy not found");
            }

            var notificationsResult = await notificationService.getNotificationForPharmacyAsync(pharmacy.DBId);
            if (!notificationsResult.IsSuccess)
            {
                return Result<List<GetNotificationDTO>>.Failure(notificationsResult.Error);
            }

            return Result<List<GetNotificationDTO>>.Success(notificationsResult.Value);
        }

        public async Task<Result> RegisterPharmacyAsync(RegisterPharmacyDTO pharmacyDTO)
        {

            // Validation Occurs oon controller by Data anotations
            // 1. Create a user account for the pharmacy
            var userResult = await userService.CreateUserAsync(pharmacyDTO.Email, pharmacyDTO.Password);

            // Add to Role 
            var roleResult = await userService.AssignUserToRole(userResult.Value, "Pharmacy");
            if(!roleResult.IsSuccess)
            {
                return Result.Failure(roleResult.Error);
            }

            if (!userResult.IsSuccess)
            {
                return Result.Failure(userResult.Error);
            }

            var Location = new Location(pharmacyDTO.Latitude, pharmacyDTO.Longitude);
            var LiscenceNumber = new LicenseNumber(pharmacyDTO.LiscenceNumber);
            var pharmacy = new Pharmacy(
                pharmacyDTO.Name,
                pharmacyDTO.Address,
                Location,
                LiscenceNumber,
                userResult.Value
            );

            // 2. Save the pharmacy to the database
            uow.Pharmacy.Create(pharmacy);
            var Result_ = await uow.SaveChangesAsync();

            if (Result_ == 0)
            {
                return Result.Failure("Failed to save pharmacy to the database.");
            }

            return Result.Success();
        }

    }
}
