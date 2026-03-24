using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Notification : Entity_
    {
        public int? PatientId { get; private set; }
        public Patient Patient { get; private set; }
        public int? PharmacyId { get; private set; }
        public Pharmacy Pharmacy { get; private set; }
        public string Message { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime CreatedAt { get; private set; }

        internal Notification() { } // For Ef Core

        internal void ForPaient(int PId, string message)
        {
            Id = Guid.NewGuid();
            PatientId = PId;
            Message = message;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }
        internal void ForPharmacy(int PhId, string message)
        {
            Id = Guid.NewGuid();
            PharmacyId = PhId;
            Message = message;
            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
