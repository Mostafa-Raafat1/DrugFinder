using Application.Common;
using Application.DTO;
using Application.Services;
using Application.UserContext;
using Domain.Entity;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DrugRequestService : IDrugRequestService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserContext userContext;

        public DrugRequestService(IUnitOfWork Uow, IUserContext userContext)
        {
            uow = Uow;
            this.userContext = userContext;
        }
        public async Task<Result> CreateDrugRequestAsync(List<CreateDrugRequestDTO> DTO)
        {
            if (DTO == null || DTO.Count == 0)
            {
                return Result.Failure("Drug request list cannot be empty.");
            }

            // Get UserAppId
            string userAppId = userContext.UserId;

            // Get PatientId from Patient table using UserAppId
            var patient = uow.Patient.getPatientByUserAppId(userAppId);

            if (patient == null)
            {
                return Result.Failure("Patient not found");
            }

            // Add DrugDetails and DrugRequest
            var drugRequestDetails = new List<DrugDetails>();

            foreach (var drug in DTO)
            {
                drugRequestDetails.Add(new DrugDetails
                (
                    drug.DrugName,
                    drug.Strength,
                    drug.Form,
                    drug.Quantity
                ));
            }
            var drugRequest = new DrugRequest(drugRequestDetails, patient.DBId, patient.Id);

            uow.DrugRequest.Create(drugRequest);

            var result = await uow.SaveChangesAsync();
            if (result == 0)
            {
                return Result.Failure("Failed to create drug request.");
            }
            else
            {
                return Result.Success();
            }
        }
    }
}
