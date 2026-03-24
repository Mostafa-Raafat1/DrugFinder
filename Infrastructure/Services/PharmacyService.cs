using Application.Common;
using Application.DTO;
using Application.Identity;
using Application.Services;
using Domain.Entity;
using Domain.Value_Object;
using Infrastructure.UnitOfWork;
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

        public PharmacyService(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
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
