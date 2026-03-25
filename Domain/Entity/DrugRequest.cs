using Domain.Domain;
using Domain.DomainEvent.Events;
using Domain.Enum;
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
        public ICollection<PharmacyResponse> PharmacyResponses { get; private set; }

        private DrugRequest() { } // For EF Core

        public DrugRequest(List<DrugDetails> drugDetails, int pId, Guid pDomainId)
        {
            Id = Guid.NewGuid();
            DrugDetails = drugDetails;
            PatientId = pId;
            RequestTime = DateTime.UtcNow;
            Status = Status.Pending;

            // Raise an Event
            AddDomainEvent(new DrugRequestedCreatedEvent(Id, pDomainId));
        }

    }
}
