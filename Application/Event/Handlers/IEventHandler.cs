using Domain.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event.Handlers
{
    public interface IEventHandler <T> where T : IDomainEvent
    {
        Task Handle(T domainEvent);
    }
}
