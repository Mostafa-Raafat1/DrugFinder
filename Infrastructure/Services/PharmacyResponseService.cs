using Application.Common;
using Application.DTO;
using Application.Services;
using Application.UserContext;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PharmacyResponseService : IPharmacyResponseService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserContext userContext;

        public PharmacyResponseService(IUnitOfWork uow, IUserContext userContext)
        {
            this.uow = uow;
            this.userContext = userContext;
        }

        public async Task<Result<PharmacyResponseDTO>> CreatePharmacyResponseAsync(PharmacyResponseDTO DTO)
        {
            if (DTO == null)
            {
                return Result<PharmacyResponseDTO>.Failure("Pharmacy response data cannot be null.");
            }

            var PharmacyResponseItems = DTO.Items.Select(i => new Domain.ValueObject.PharmacyResponseItem
            (
                i.DrugName,
                i.Available,
                i.Price
            )).ToList();

            var Pharmacy = await uow.Pharmacy.GetPharmacyByUserIdAsync(userContext.UserId);

            var pharmacyResponse = new Domain.Entity.PharmacyResponse
            (
                DTO.RequestId
,               Pharmacy.DBId,
                PharmacyResponseItems
            );

            uow.PharamcyResponse.Create(pharmacyResponse);
            if (await uow.SaveChangesAsync() == 0)
            {
                return Result<PharmacyResponseDTO>.Failure("Failed to create pharmacy response.");
            }

            
            return Result<PharmacyResponseDTO>.Success(DTO);
        }
    }
}
