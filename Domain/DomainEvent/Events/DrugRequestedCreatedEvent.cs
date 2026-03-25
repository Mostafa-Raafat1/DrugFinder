using Domain.Value_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvent.Events
{
    public class DrugRequestedCreatedEvent : IDomainEvent
    {
        public Guid RequestId { get;}
        public Guid PatientId { get; }
        public DrugRequestedCreatedEvent(Guid RID, Guid PID)
        {
            RequestId = RID;
            PatientId = PID;
        }
    }
}
