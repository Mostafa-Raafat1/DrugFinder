using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PharmacyResponse : Entity_
    {
        public int DrugRequestId { get; private set; }
        public DrugRequest DrugRequest { get; private set; } // Nvigation property
        public int PharmacyId { get; private set; }
        public Pharmacy Pharmacy { get; private set; } // Navigation property
        public AvailabilityStatus AvailabilityStatus { get; private set; }
        public Money Price { get; private set; }
        public DateTime ResponseTime { get; private set; }

        private PharmacyResponse() { } // For EF Core

        public PharmacyResponse(int dRId, int PhId, AvailabilityStatus status, Money price)
        {
            Id = Guid.NewGuid();
            DrugRequestId = dRId;
            PharmacyId = PhId;
            AvailabilityStatus = status;
            Price = price;
            ResponseTime = DateTime.UtcNow;
        }

        public void MarkAsUnavailable()
        {
            AvailabilityStatus = AvailabilityStatus.Unavailable;
        }

        public void MarkAsAvailable()
        {
            AvailabilityStatus = AvailabilityStatus.Available;
        }

        public void updatePrice(Money newPrice)
        {
            Price = newPrice;
        }

        public void UpdateResponse(AvailabilityStatus newStatus, Money newPrice)
        {
            AvailabilityStatus = newStatus;
            Price = newPrice;
            ResponseTime = DateTime.UtcNow; // Update response time to now
        }

        public void setPharmacy(Pharmacy pharmacy)
        {
            Pharmacy = pharmacy;
            PharmacyId = pharmacy.DBId;
        }
        public void setDrugRequest(DrugRequest drugRequest)
        {
            DrugRequest = drugRequest;
            DrugRequestId = drugRequest.DBId;
        }

    }
}
