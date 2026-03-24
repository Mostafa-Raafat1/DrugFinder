using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Pharmacy : Entity_
    {
        public string AppUserId { get; private set; }
        public string PharmacyName { get; private set; }
        public string Address { get; private set; }
        public Location Location { get; private set; }
        public LicenseNumber LicenseNumber { get; private set; }
        public DateTime CeatedAt { get; private set; }
        public ICollection<PharmacyResponse> PharmacyResponses { get; private set; }
        public ICollection<Notification> Notifications { get; private set; }

        private Pharmacy() { } // For EF Core

        public Pharmacy(string pName, string address, Location location, LicenseNumber license, string appUserId)
        {
            Id = Guid.NewGuid();
            PharmacyName = pName;
            Address = address;
            Location = location;
            LicenseNumber = license;
            CeatedAt = DateTime.UtcNow;
            AppUserId = appUserId;
        }

        public void UpdatePharmacyInfo(string newName, string newAddress, Location newLocation, LicenseNumber newLicense)
        {
            PharmacyName = newName;
            Address = newAddress;
            Location = newLocation;
            LicenseNumber = newLicense;
        }

        public void AddPharmacyResponse(PharmacyResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (!CanResponseBeAdded(response))
                throw new InvalidOperationException("This response has already been added to the pharmacy.");
            PharmacyResponses.Add(response);
        }
        
        private bool CanResponseBeAdded(PharmacyResponse response)
        {
            return !PharmacyResponses.Any(r => r.Id == response.Id);
        }


    }

}
