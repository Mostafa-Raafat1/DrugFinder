using Domain.Enum;
using Domain.ValueObject;
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
        public List<PharmacyResponseItem> ResponseItems { get; private set; } // List of response items for each drug in the request 


        private PharmacyResponse() { } // For EF Core

        public PharmacyResponse(int dRId, int PhId, List<PharmacyResponseItem> PRI)
        {
            Id = Guid.NewGuid();
            DrugRequestId = dRId;
            PharmacyId = PhId;
            ResponseTime = DateTime.UtcNow;
            ResponseItems = PRI;
            setAvailabilityStatus();

            var total = ResponseItems
                        .Where(item => item.Price != null)
                        .Sum(item => item.Price!.Value); // Use ! if Price is nullable but we filtered nulls

            Price = new Money(total, Currency.EGP);
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

        private void setAvailabilityStatus()
        {
            if (ResponseItems.All (item => !item.Available))
            {
                AvailabilityStatus = AvailabilityStatus.Unavailable; // If all items are unavailable, mark the whole response as unavailable
            }
            else if (ResponseItems.Any(item => !item.Available))
            {
                AvailabilityStatus = AvailabilityStatus.PartiallyAvailable; // If at least one item is unavailable, but not all, mark the response as partially available
            }
            else
            {
                AvailabilityStatus = AvailabilityStatus.Available; // If all items are available, mark the response as available
            }
        }

    }
}
