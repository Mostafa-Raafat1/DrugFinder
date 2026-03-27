using Domain.Domain;
using Domain.DomainEvent.Events;
using Domain.Enum;
using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class DrugRequest : Entity_
    {
        public List<DrugDetails> DrugDetails { get; private set; }
        public int PatientId { get; private set; }
        public Patient Patient { get; private set; }
        public DateTime RequestTime { get; private set; }
        public Status Status { get; private set; }
        public Location Location { get; private set; } // data duplication but for fast query
        public string PatientName { get; private set; } // data duplication but for fast query
        public ICollection<PharmacyResponse> PharmacyResponses { get; private set; }

        private DrugRequest() { } // For EF Core

        public DrugRequest(List<DrugDetails> drugDetails, int pId, Guid pDomainId, Location location, string patientName)
        {
            Id = Guid.NewGuid();
            DrugDetails = drugDetails;
            PatientId = pId;
            RequestTime = DateTime.UtcNow;
            Status = Status.Pending;
            Location = location;
            PatientName = patientName;

            // Raise an Event
            AddDomainEvent(new DrugRequestedCreatedEvent(Id, pDomainId));
            PatientName = patientName;
        }

    }
}
