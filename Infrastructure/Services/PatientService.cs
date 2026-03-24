using Application.Common;
using Application.DTO;
using Application.Identity;
using Application.Services;
using Domain.Domain;
using Domain.Value_Object;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserService userService;

        public PatientService(IUnitOfWork uow, IUserService userService)
        {
            this.uow = uow;
            this.userService = userService;
        }
        public async Task<Result> RegisterPatientAsync(RegitserPatientDTO DTO)
        {
            // Create Patient User
            var result = await userService.CreateUserAsync(DTO.Email, DTO.Password);
            if (!result.IsSuccess)
            {
                return Result.Failure(result.Error);
            }

            // Add User To Role
            var roleResult = await userService.AssignUserToRole(result.Value, "Patient");
            if (!roleResult.IsSuccess)
            {
                return Result.Failure(roleResult.Error);
            }

            // Create Patient Entity
            var Location = new Location(DTO.Latitude, DTO.Longitude);
            var Patient = new Patient(DTO.FName,
                                      DTO.SName,
                                      Location,
                                      result.Value);

            uow.Patient.Create(Patient);
            var entityResult = await uow.SaveChangesAsync();
            if(entityResult == 0)
            {
                return Result.Failure("Error while Registering Patient");
            }

            return Result.Success();
        }

    }
}
