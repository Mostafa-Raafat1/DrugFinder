using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Domain
{
    public class Patient : Entity_
    {
        public string AppUserId { get; private set; }
        public string FName { get; private set; }
        public string SName { get; private set; }
        public Location Location { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<Notification> Notifications { get; private set; }
        public ICollection<DrugRequest> DrugRequests { get; private set; }

        private Patient() { } // For EF Core

        // ctor for creating a new patient
        public Patient(string fname, string sname, Location loc, string appUserId)
        {
            // Make a Domain Id
            Id = Guid.NewGuid();
            FName = fname;
            SName = sname;
            Location = loc;
            CreatedAt = DateTime.UtcNow;
            AppUserId = appUserId;

        }

        

    }
}
