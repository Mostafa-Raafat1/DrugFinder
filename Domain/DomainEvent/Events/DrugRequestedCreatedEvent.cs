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
        public int RequestId { get;}
        public int PatientId { get; }
        public DrugRequestedCreatedEvent(int RID, int PID)
        {
            RequestId = RID;
            PatientId = PID;
        }
    }
}
